using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Linq;

namespace Faza
{
    public class EnemyInput : CharacterInput
    {
        private const string JUMP_COMMAND = "jump";

        [SerializeField] private LayerMask _aggroMask;
        [SerializeField] private LayerMask _visibilityMask;
        [SerializeField] private MeleeAttack _meleeAttack;
        [SerializeField] private float _sensitivity;
        [SerializeField] private Transform _look;
        [SerializeField] private float _stoppingDistance;
        [SerializeField] private float _turningCap;
        [SerializeField] private GameObject _fpCamera;
        [SerializeField] private GameObject _tpCamera;
        [SerializeField] private float _jumpCheckDelta = 0.1f;
        [SerializeField] private float _jumpCheckDistance = 1f;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Health _health;
        [SerializeField] private float _visionRadius;
        [SerializeField] private Transform[] _patrolPoints;

        private float _cameraX;
        private float _cameraY;
        private float _horizontal;
        private float _vertical;
        private bool _jump;
        private bool _use;

        private static HashSet<EnemyInput> _allEnemies = new();

        private IWaypoint _currentWaypoint;
        private Queue<IWaypoint> _currentPath;
        private int _currentPathIndex;
        private Vector3 _oldCorner;
        private Collider[] _colliders = new Collider[32];
        private int _patrolIndex;

        public GameObject FpCamera => _fpCamera;
        public GameObject TpCamera => _tpCamera;

        public Health Health => _health;

        public static bool IsEveryoneDead => _allEnemies.All(x => x.Health.IsDead);

        public static HashSet<EnemyInput> AllEnemies => _allEnemies;

        private void Awake()
        {
            _health.OnDeath += Health_OnDeath;

            _agent.updateRotation = false;
            _agent.updatePosition = false;

            StartCoroutine(FollowPlayerCoroutine());

            _allEnemies.Add(this);
        }

        private void OnDestroy()
        {
            _allEnemies.Remove(this);
        }

        private void Update()
        {
            MoveByUnityAgent();
        }

        public override float GetCameraX()
        {
            return _cameraX * _sensitivity;
        }

        public override float GetCameraY()
        {
            return _cameraY * _sensitivity;
        }

        public override Vector3 GetMove()
        {
            return new Vector3(_horizontal, 0f, _vertical);
        }

        public override bool GetJump()
        {
            var value = _jump;
            _jump = false;
            return value;
        }

        public override bool GetUse()
        {
            return _use;
        }

        public void SetCameraX(float value)
        {
            _cameraX = value;
        }

        public void SetCameraY(float value)
        {
            _cameraY = value;
        }

        public void SetHorizontal(float value)
        {
            _horizontal = value;
        }

        public void SetVertical(float value)
        {
            _vertical = value;
        }

        public void SetJump(bool value)
        {
            _jump = value;
        }

        public void SetUse(bool value)
        {
            _use = value;
        }

        public void SetWaypoint(Waypoint waypoint)
        {
            _currentWaypoint = waypoint;

            if (_currentWaypoint == null)
            {
                _cameraX = 0f;
                _horizontal = 0f;
                _vertical = 0f;
            }
        }

        public void SetDestination(Vector3 destination)
        {
            var from = Waypoint.Closest(transform.position);
            var to = Waypoint.Closest(destination);

            _currentPath = new Queue<IWaypoint>(from.FindPath(to));

            foreach (var wp in Waypoint.All)
            {
                wp.IsMarked = false;
            }

            foreach (var wp in _currentPath)
            {
                wp.IsMarked = true;
            }

            _currentPath.Enqueue(new ScriptedWaypoint(destination));

            _currentWaypoint = _currentPath.Dequeue();
            if (NeedsToJump(transform.position)) _jump = true;
        }

        public override bool IsFire()
        {
            return false;
        }

        private bool NeedsToJump(Vector3 pos)
        {
            var targetPos = _currentWaypoint.Pos;
            var direction = (targetPos - pos).normalized;
            var distance = Vector3.Distance(pos, targetPos);
            var current = 0f;

            while (current < distance)
            {
                var from = pos + direction * current;
                var ray = new Ray(from, Vector3.down);
                var hit = Physics.Raycast(ray, _jumpCheckDistance);
                if (EntryPoint.IsDebugOn)
                {
                    Line.DrawRayPermanent(from, Vector3.down * _jumpCheckDistance, Color.green);
                }
                if (hit == false) return true;
                current += _jumpCheckDelta;
            }

            return false;
        }

        private void MoveByCustomWaypoints()
        {
            if (_currentWaypoint == null) return;

            var lookForward = _look.forward.WithY(0f).normalized;
            var wpPosition = _currentWaypoint.Pos.WithY(0f);
            var position = transform.position.WithY(0f);

            var distanceToWp = (wpPosition - position).sqrMagnitude;
            if (distanceToWp < _stoppingDistance)
            {
                var command = _currentWaypoint.Command.ToLower();
                if (command == JUMP_COMMAND)
                {
                    _jump = true;
                }

                if (_currentPath.Count == 0)
                {
                    _currentWaypoint = null;
                }
                else
                {
                    var pos = _currentWaypoint.Pos;
                    _currentWaypoint = _currentPath.Dequeue();
                    if (NeedsToJump(pos)) _jump = true;
                }

                _cameraX = 0f;
                _horizontal = 0f;
                _vertical = 0f;
                return;
            }

            var directionToWp = (wpPosition - position).normalized;

            var cross = Vector3.Cross(lookForward, directionToWp);
            var dot = Vector3.Dot(lookForward, directionToWp);

            if (EntryPoint.IsDebugOn)
            {
                Line.DrawRay(transform.position, lookForward, Color.blue);
                Line.DrawRay(transform.position, directionToWp, Color.green);
                Line.DrawRay(transform.position, cross, Color.yellow);
            }

            if (dot < -0.95f)
            {
                _cameraX = -1f * Time.deltaTime;
            }
            else
            {
                _cameraX = cross.y.Abs() < _turningCap ? 0f : Mathf.Sign(cross.y) * Time.deltaTime;
            }

            var localDirectionToWp = _look.InverseTransformDirection(directionToWp);

            _horizontal = localDirectionToWp.x;
            _vertical = localDirectionToWp.z;
        }

        private void MoveByUnityAgent()
        {
            if (_agent.pathPending) return;

            _agent.nextPosition = transform.position;

            var currentWp = _agent.steeringTarget;
            var lookForward = _look.forward.WithY(0f).normalized;
            var position = transform.position.WithY(0f);
            var wpPosition = currentWp.WithY(0f);

            var directionToWp = (wpPosition - position).normalized;

            var cross = Vector3.Cross(lookForward, directionToWp);
            var dot = Vector3.Dot(lookForward, directionToWp);

            if (EntryPoint.IsDebugOn)
            {
                Line.DrawRay(transform.position, lookForward, Color.blue);
                Line.DrawRay(transform.position, directionToWp, Color.green);
                Line.DrawRay(transform.position, cross, Color.yellow);
            }

            var camX = 0f;
            if (dot < -0.95f)
            {
                camX = -1f;
            }
            else
            {
                camX = cross.y;
            }
            _cameraX = camX * Time.deltaTime;

            var localDirectionToWp = _look.InverseTransformDirection(directionToWp);

            _horizontal = localDirectionToWp.x;
            _vertical = localDirectionToWp.z;
        }

        private void Health_OnDeath()
        {
            enabled = false;
        }

        private IEnumerator FollowPlayerCoroutine()
        {
            var playerFound = false;

            while (true)
            {
                yield return null;

                if (_meleeAttack.WithinAttack)
                {
                    _agent.ResetPath();
                    continue;
                }

                var amount = Physics.OverlapSphereNonAlloc
                    (transform.position, _visionRadius, _colliders, _aggroMask.value);
                for (var i = 0; i < amount; i++)
                {
                    var player = _colliders[i].GetComponent<PlayerInput>();

                    if (player == false) continue;
                    if (player.Health.IsDead) continue;

                    var vector = (player.transform.position - transform.position);

                    var ray = new Ray(transform.position.DeltaY(0.5f), vector.normalized);
                    var hit = Physics.Raycast(ray, out var hitInfo, _visionRadius, _visibilityMask);
                    if (hit == false) continue;

                    if (hitInfo.collider.GetComponent<PlayerInput>() == false) continue;

                    _agent.SetDestination(PlayerInput.Instance.transform.position);
                    playerFound = true;
                    break;
                }

                if (playerFound == false && _patrolPoints != null && _patrolPoints.Length != 0)
                {
                    var nextPatrolPoint = _patrolPoints[_patrolIndex].position;
                    _agent.SetDestination(nextPatrolPoint);

                    if (Vector3.Distance(transform.position, nextPatrolPoint) < 1f)
                    {
                        _patrolIndex++;
                        _patrolIndex %= _patrolPoints.Length;
                    }
                }
                else if (playerFound == false)
                {
                    _agent.ResetPath();
                }

                playerFound = false;
            }
        }
    }
}

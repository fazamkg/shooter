using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Faza
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        [SerializeField] private CharacterInput _characterInput;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _camera;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _friction;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _gravity;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _useDistance;
        [SerializeField] private bool _cameraBasedInput = true;

        private bool _isNoclip;

        private float _pitch;
        private float _yaw;

        private Vector3 _ownHorizontalVelocity;
        private Vector3 _outsideHorizontalVelocity;
        private float _verticalVelocity;
        private bool _isGrounded;
        private Coroutine _stopCoroutine;

        public bool DeltaTimeScaled { get; set; } = true;
        public float HorizontalSpeed => _ownHorizontalVelocity.magnitude;
        public float Yaw => _yaw;
        public bool IsFalling => _isGrounded == false && _verticalVelocity < 0f;
        public bool IsGrouned => _isGrounded;
        public bool IsNoclip
        {
            get => _isNoclip;
            set
            {
                _isNoclip = value;
                _characterController.enabled = !value;
            }
        }
        public CharacterController CharacterController => _characterController;

        public void AddSpeed(float value)
        {
            _acceleration += value;
            _maxSpeed += value;
        }

        private void Awake()
        {
            _characterController.enableOverlapRecovery = false;
        }

        private void Update()
        {
            if (_characterController.enabled == false) return;

            var delta = DeltaTimeScaled ? Time.deltaTime : Time.unscaledDeltaTime;

            var camX = _characterInput.GetCameraX();
            var camY = _characterInput.GetCameraY();

            _pitch += camY;
            _pitch = Mathf.Clamp(_pitch, -90f, 90f);

            _yaw += camX;
            _yaw = Mathf.Repeat(_yaw, 360f);

            _camera.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

            var input = _characterInput.GetMove();

            var inputDirection = _cameraBasedInput ? Quaternion.Euler(0f, _yaw, 0f) * input : input;

            if (IsNoclip == false)
            {
                #region Physics
                _ownHorizontalVelocity += delta * _acceleration * inputDirection;
                _ownHorizontalVelocity -= delta * _friction * _ownHorizontalVelocity;
                _outsideHorizontalVelocity -= delta * _friction * _outsideHorizontalVelocity;
                _ownHorizontalVelocity = Vector3.ClampMagnitude(_ownHorizontalVelocity, _maxSpeed);

                _verticalVelocity -= delta * _gravity;

                _characterController.Move(_ownHorizontalVelocity * delta);
                var returnedVelocity = _characterController.velocity;
                _ownHorizontalVelocity.x = returnedVelocity.x;
                _ownHorizontalVelocity.z = returnedVelocity.z;

                _characterController.Move(_outsideHorizontalVelocity * delta);
                var returnedVelocity2 = _characterController.velocity;
                _outsideHorizontalVelocity.x = returnedVelocity2.x;
                _outsideHorizontalVelocity.z = returnedVelocity2.z;

                _characterController.Move(_verticalVelocity * delta * Vector3.up);
                returnedVelocity = _characterController.velocity;
                _verticalVelocity = returnedVelocity.y;

                _isGrounded = _characterController.isGrounded;

                if (_isGrounded && _characterInput.GetJump())
                {
                    _verticalVelocity = _jumpSpeed;
                }
                #endregion
            }
            else
            {
                var dir = Quaternion.Euler(_pitch, _yaw, 0f) * input;
                transform.position += _maxSpeed * delta * dir;
            }

            if (_characterInput.GetUse())
            {
                var hit = GetCrosshairInfo(out var hitInfo);

                if (hit)
                {
                    var worldButton = hitInfo.transform.GetComponent<WorldButton>();
                    if (worldButton != null)
                    {
                        worldButton.Use();
                    }
                }
            }
        }

        public void Stop(float duration)
        {
            if (_stopCoroutine != null)
            {
                StopCoroutine(_stopCoroutine);
            }
            _stopCoroutine = StartCoroutine(StopCharacterCoroutine(duration));
        }

        private IEnumerator StopCharacterCoroutine(float duration)
        {
            _ownHorizontalVelocity = Vector3.zero;
            _outsideHorizontalVelocity = Vector3.zero;
            enabled = false;
            yield return new WaitForSeconds(duration);
            enabled = true;
            _ownHorizontalVelocity = Vector3.zero;
            _outsideHorizontalVelocity = Vector3.zero;
        }

        public void ResetSpeeds()
        {
            _ownHorizontalVelocity = Vector3.zero;
            _outsideHorizontalVelocity = Vector3.zero;
        }

        public IEnumerator RotateTowardsCoroutine(Vector3 target, float speed)
        {
            target = target.WithY(0f);
            var crossY = 1f;
            var dot = -1f;

            while (crossY.Abs() > 0.1f || dot < -0.95f)
            {
                var lookForward = _camera.transform.forward;
                var direction = (target - transform.position.WithY(0f)).normalized;

                var targetYaw = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);

                var cross = Vector3.Cross(lookForward, direction);
                dot = Vector3.Dot(lookForward, direction);
                var result = 0f;
                crossY = cross.y;

                if (dot < -0.95f)
                {
                    result = -1f;
                }
                else
                {
                    result = crossY;
                }

                var delta = result * speed * Time.deltaTime;
                var maxDelta = Mathf.DeltaAngle(_yaw, targetYaw);
                var min = Mathf.Min(delta.Abs(), maxDelta.Abs());
                delta = min * delta.Sign();

                _yaw += delta;
                _yaw = Mathf.Repeat(_yaw, 360f);

                yield return null;
            }
        }

        public bool IsAboutToLand()
        {
            var ray = new Ray(transform.position, Vector3.down);
            return Physics.Raycast(ray, out var hitInfo, 2f);
        }

        public bool GetCrosshairInfo(out RaycastHit hitInfo)
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);
            return Physics.Raycast(ray, out hitInfo, _useDistance);
        }

        public void SetAccelration(float value)
        {
            _acceleration = value;
        }

        public void SetFriction(float value)
        {
            _friction = value;
        }

        public void SetMaxSpeed(float value)
        {
            _maxSpeed = value;
        }

        public void AddVelocity(Vector3 velocity)
        {
            _outsideHorizontalVelocity += velocity;
        }

        public void Warp(Vector3 position)
        {
            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
        }

        public Tween SmoothWarp(Vector3 position, float duration)
        {
            var seq = DOTween.Sequence();

            seq.AppendCallback(() => _characterController.enabled = false);
            seq.Append(transform.DOMove(position, duration));
            seq.AppendCallback(() => _characterController.enabled = true);
            _characterController.enabled = true;

            return seq;
        }
    }
}

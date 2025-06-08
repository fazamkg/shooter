using UnityEngine;

namespace Faza
{
    public class EnemyInput : CharacterInput
    {
        [SerializeField] private float _sensitivity;
        [SerializeField] private Transform _look;
        [SerializeField] private float _stoppingDistance;
        [SerializeField] private float _turningCap;

        private float _cameraX;
        private float _cameraY;
        private float _horizontal;
        private float _vertical;
        private bool _jump;
        private bool _use;

        private Waypoint _currentWaypoint;

        public override float GetCameraX()
        {
            return _cameraX * _sensitivity;
        }

        public override float GetCameraY()
        {
            return _cameraY * _sensitivity;
        }

        public override float GetHorizontal()
        {
            return _horizontal;
        }

        public override float GetVertical()
        {
            return _vertical;
        }

        public override bool GetJump()
        {
            return _jump;
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
        }

        private void Update()
        {
            if (_currentWaypoint == null) return;

            var lookForward = _look.forward.WithY(0f);
            var wpPosition = _currentWaypoint.transform.position.WithY(0f);
            var position = transform.position.WithY(0f);

            var distanceToWp = (wpPosition - position).sqrMagnitude;
            if (distanceToWp < _stoppingDistance)
            {
                _currentWaypoint = _currentWaypoint.Next;
                _cameraX = 0f;
                _horizontal = 0f;
                _vertical = 0f;
                return;
            }

            var directionToWp = (wpPosition - position).normalized;

            var cross = Vector3.Cross(lookForward, directionToWp);
            _cameraX = cross.y.Abs() < _turningCap ? 0f : Mathf.Sign(cross.y);

            var localDirectionToWp = _look.InverseTransformDirection(directionToWp);

            _horizontal = localDirectionToWp.x;
            _vertical = localDirectionToWp.z;
        }
    }
}

using UnityEngine;
using System.Collections;

namespace Faza
{
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

        private Vector3 _velocity;
        private float _verticalVelocity;
        private bool _isGrounded;

        public bool DeltaTimeScaled { get; set; } = true;
        public float HorizontalSpeed => _velocity.magnitude;
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

        private void Update()
        {
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
                _velocity += delta * _acceleration * inputDirection;

                _velocity -= delta * _friction * _velocity;

                _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);

                _verticalVelocity -= delta * _gravity;

                _characterController.Move(_velocity * delta);
                var returnedVelocity = _characterController.velocity;
                _velocity.x = returnedVelocity.x;
                _velocity.z = returnedVelocity.z;

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

        public IEnumerator RotateTowardsCoroutine(Vector3 target, float speed)
        {
            target = target.WithY(0f);
            var crossY = 1f;
            var dot = -1f;

            while (crossY.Abs() > 0.1f || dot < -0.95f)
            {
                var lookForward = _camera.transform.forward;
                var direction = (target - transform.position.WithY(0f)).normalized;

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

                _yaw += result * speed * Time.deltaTime;

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
    }
}

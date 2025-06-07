using UnityEngine;

namespace Faza
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private CharacterInput _characterInput;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _camera;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _friction;
        [SerializeField] private float _gravity;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _useDistance;

        private float _pitch;
        private float _yaw;

        private Vector3 _velocity;
        private float _verticalVelocity;

        public float HorizontalSpeed => _velocity.magnitude;
        public float Yaw => _yaw;

        private void Update()
        {
            var camX = _characterInput.GetCameraX();
            var camY = _characterInput.GetCameraY();

            _pitch += camY;
            _pitch = Mathf.Clamp(_pitch, -90f, 90f);

            _yaw += camX;
            _yaw = Mathf.Repeat(_yaw, 360f);

            _camera.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

            var horizontal = _characterInput.GetHorizontal();
            var vertical = _characterInput.GetVertical();

            var inputDirection = new Vector3(horizontal, 0f, vertical);

            inputDirection = Quaternion.Euler(0f, _yaw, 0f) * inputDirection;

            _velocity += Time.deltaTime * _acceleration * inputDirection;

            _velocity -= Time.deltaTime * _friction * _velocity;

            _verticalVelocity -= Time.deltaTime * _gravity;

            _characterController.Move(_velocity * Time.deltaTime);
            var returnedVelocity = _characterController.velocity;
            _velocity.x = returnedVelocity.x;
            _velocity.z = returnedVelocity.z;

            _characterController.Move(_verticalVelocity * Time.deltaTime * Vector3.up);
            returnedVelocity = _characterController.velocity;
            _verticalVelocity = returnedVelocity.y;

            var isGrounded = _characterController.isGrounded;

            if (isGrounded && _characterInput.GetJump())
            {
                _verticalVelocity = _jumpSpeed;
            }

            if (_characterInput.GetUse())
            {
                var ray = new Ray(_camera.transform.position, _camera.transform.forward);
                var hit = Physics.Raycast(ray, out var hitInfo, _useDistance);

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
    } 
}

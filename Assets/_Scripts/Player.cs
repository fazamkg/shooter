using UnityEngine;

namespace Faza
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _camera;
        [SerializeField] private float _sensitivity;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _friction;
        [SerializeField] private float _gravity;
        [SerializeField] private float _jumpSpeed;

        private float _pitch;
        private float _yaw;

        private Vector3 _velocity;
        private float _verticalVelocity;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            var mouseX = Input.GetAxisRaw("Mouse X");
            var mouseY = Input.GetAxisRaw("Mouse Y");

            _pitch -= mouseY * _sensitivity;
            _pitch = Mathf.Clamp(_pitch, -90f, 90f);

            _yaw += mouseX * _sensitivity;
            _yaw = Mathf.Repeat(_yaw, 360f);

            _camera.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

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

            if (isGrounded && Input.GetButton("Jump"))
            {
                _verticalVelocity = _jumpSpeed;
            }
        }
    }
}

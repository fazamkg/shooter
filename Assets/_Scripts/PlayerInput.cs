using UnityEngine;

namespace Faza
{
    public class PlayerInput : CharacterInput
    {
        [SerializeField] private float _sensitivity;
        [SerializeField] private GameObject _camera;
        [SerializeField] private float _turningCap;
        [SerializeField] private float _rotationSpeed;

        private bool _locked;

        public GameObject Camera => _camera;

        private void Awake()
        {
            _locked = true;
            Console.StartReadingBinds();
        }

        private void Start()
        {
            CameraTracker.Activate("main");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _locked = !_locked;

                if (_locked)
                {
                    Console.StartReadingBinds();
                    Console.CloseConsole();
                }
                else
                {
                    Console.StopReadingBinds();
                    Console.OpenConsole();
                }
            }
        }

        public override float GetCameraX()
        {
            if (_locked == false) return 0f;

            var joyV = Joystick.GetInput("move").y;
            var kbV = Input.GetAxisRaw("Vertical");
            var vertical = Mathf.Min(1f, joyV + kbV);

            var joyH = Joystick.GetInput("move").x;
            var kbH = Input.GetAxisRaw("Horizontal");
            var horizontal = Mathf.Min(1f, joyH + kbH);

            var lookForward = _camera.transform.forward;
            var direction = new Vector3(horizontal, 0f, vertical).normalized;

            var cross = Vector3.Cross(lookForward, direction);
            var dot = Vector3.Dot(lookForward, direction);
            var result = 0f;

            if (dot < -0.95f)
            {
                result = -1f;
            }
            else
            {
                result = cross.y.Abs() < _turningCap ? 0f : Mathf.Sign(cross.y);
            }

            return result * Time.deltaTime * _rotationSpeed;
        }

        public override float GetCameraY()
        {
            return 0f;
        }

        public override float GetHorizontal()
        {
            if (_locked == false) return 0f;

            var joy = Joystick.GetInput("move").x;
            var kb = Input.GetAxisRaw("Horizontal");

            return Mathf.Min(1f, joy + kb);
        }

        public override float GetVertical()
        {
            if (_locked == false) return 0f;

            var joy = Joystick.GetInput("move").y;
            var kb = Input.GetAxisRaw("Vertical");

            return Mathf.Min(1f, joy + kb);
        }

        public override bool GetJump()
        {
            return false;
        }

        public override bool GetUse()
        {
            return false;
        }

        public override bool IsFire()
        {
            return false;
        }
    }
}

using UnityEngine;

namespace Faza
{
    public class PlayerInput : CharacterInput
    {
        [SerializeField] private float _sensitivity;
        [SerializeField] private GameObject _camera;

        private bool _locked;

        public GameObject Camera => _camera;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Console.StartReadingBinds();
                    Console.CloseConsole();
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Console.StopReadingBinds();
                    Console.OpenConsole();
                }
            }
        }

        public override float GetCameraX()
        {
            if (_locked == false) return 0f;

            return Input.GetAxisRaw("Mouse X") * _sensitivity;
        }

        public override float GetCameraY()
        {
            if (_locked == false) return 0f;

            return -Input.GetAxisRaw("Mouse Y") * _sensitivity;
        }

        public override float GetHorizontal()
        {
            if (_locked == false) return 0f;

            return Input.GetAxisRaw("Horizontal");
        }

        public override float GetVertical()
        {
            if (_locked == false) return 0f;

            return Input.GetAxisRaw("Vertical");
        }

        public override bool GetJump()
        {
            if (_locked == false) return false;

            return Input.GetButton("Jump");
        }

        public override bool GetUse()
        {
            if (_locked == false) return false;

            return Input.GetKeyDown(KeyCode.E);
        }
    }
}

using UnityEngine;

namespace Faza
{
    public class PlayerInput : CharacterInput
    {
        [SerializeField] private float _sensitivity;

        private bool _locked;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _locked = true;
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
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
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
            return Input.GetAxisRaw("Horizontal");
        }

        public override float GetVertical()
        {
            return Input.GetAxisRaw("Vertical");
        }

        public override bool GetJump()
        {
            return Input.GetButton("Jump");
        }

        public override bool GetUse()
        {
            return Input.GetKeyDown(KeyCode.E);
        }
    }
}

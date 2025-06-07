using UnityEngine;

namespace Faza
{
    public class PlayerInput : CharacterInput
    {
        [SerializeField] private float _sensitivity;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public override float GetCameraX()
        {
            return Input.GetAxisRaw("Mouse X") * _sensitivity;
        }

        public override float GetCameraY()
        {
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
    }
}

using UnityEngine;

namespace Faza
{
    public class EnemyInput : CharacterInput
    {
        private float _cameraX;
        private float _cameraY;
        private float _horizontal;
        private float _vertical;
        private bool _jump;
        private bool _use;

        public override float GetCameraX()
        {
            return _cameraX;
        }

        public override float GetCameraY()
        {
            return _cameraY;
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
    }
}

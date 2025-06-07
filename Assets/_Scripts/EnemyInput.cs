using UnityEngine;

namespace Faza
{
    public class EnemyInput : CharacterInput
    {
        public override float GetCameraX()
        {
            return 0f;
        }

        public override float GetCameraY()
        {
            return 0f;
        }

        public override float GetHorizontal()
        {
            return 0f;
        }

        public override float GetVertical()
        {
            return 0f;
        }

        public override bool GetJump()
        {
            return false;
        }
    }
}

using UnityEngine;

namespace Faza
{
    public abstract class CharacterInput : MonoBehaviour
    {
        public abstract float GetCameraX();
        public abstract float GetCameraY();

        public abstract Vector3 GetMove();

        public abstract bool GetJump();

        public abstract bool GetUse();

        public abstract bool IsFire();
    }
}

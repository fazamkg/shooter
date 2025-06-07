using UnityEngine;

namespace Faza
{
    public abstract class CharacterInput : MonoBehaviour
    {
        public abstract float GetCameraX();
        public abstract float GetCameraY();

        public abstract float GetHorizontal();
        public abstract float GetVertical();

        public abstract bool GetJump();

        public abstract bool GetUse();
    } 
}

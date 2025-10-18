using UnityEngine;

namespace Faza
{
    public static class AnimatorKey
    {
        public static readonly int IdleVariant = Animator.StringToHash("IdleVariant");
        public static readonly int InterruptAttack = Animator.StringToHash("InterruptAttack");
        public static readonly int InputHorizontal = Animator.StringToHash("InputHorizontal");
        public static readonly int InputVertical = Animator.StringToHash("InputVertical");
        public static readonly int MouseX = Animator.StringToHash("MouseX");
        public static readonly int Vertical = Animator.StringToHash("Vertical");
        public static readonly int HorizontalSpeed = Animator.StringToHash("HorizontalSpeed");
        public static readonly int ShootSpeed = Animator.StringToHash("ShootSpeed");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int MeleeAttack = Animator.StringToHash("MeleeAttack");
        public static readonly int OpenChest = Animator.StringToHash("OpenChest");
        public static readonly int Hit = Animator.StringToHash("Hit");
        public static readonly int Death = Animator.StringToHash("Death");
    } 
}

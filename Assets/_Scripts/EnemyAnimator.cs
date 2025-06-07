using UnityEngine;

namespace Faza
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private Animator _animator;

        private void Update()
        {
            _animator.SetFloat("HorizontalSpeed", _character.HorizontalSpeed);
        }
    } 
}

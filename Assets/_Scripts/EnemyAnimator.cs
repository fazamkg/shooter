using UnityEngine;

namespace Faza
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private CharacterInput _input;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _smoothSpeed;

        private float _horizontal;
        private float _vertical;

        private void Update()
        {
            var horizontal = _input.GetHorizontal();
            var vertical = _input.GetVertical();

            _horizontal = Mathf.MoveTowards(_horizontal, horizontal, Time.deltaTime * _smoothSpeed);
            _vertical = Mathf.MoveTowards(_vertical, vertical, Time.deltaTime * _smoothSpeed);

            _animator.SetFloat("InputHorizontal", _horizontal);
            _animator.SetFloat("InputVertical", _vertical);
        }
    } 
}

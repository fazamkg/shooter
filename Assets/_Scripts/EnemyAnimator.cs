using UnityEngine;

namespace Faza
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private CharacterInput _input;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _smoothSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _minSpeed;

        private float _horizontal;
        private float _vertical;
        private float _cameraX;
        private float _y;
        private float _horizontalSpeed;

        private void Update()
        {
            var horizontal = _input.GetHorizontal();
            var vertical = _input.GetVertical();

            _horizontal = Mathf.MoveTowards(_horizontal, horizontal, Time.deltaTime * _smoothSpeed);
            _vertical = Mathf.MoveTowards(_vertical, vertical, Time.deltaTime * _smoothSpeed);

            _animator.SetFloat("InputHorizontal", _horizontal);
            _animator.SetFloat("InputVertical", _vertical);

            var cameraX = _input.GetCameraX();
            if (cameraX > 0.01f)
            {
                cameraX = 1f;
            }
            else if (cameraX < -0.01f)
            {
                cameraX = -1f;
            }

            _cameraX = Mathf.MoveTowards(_cameraX, cameraX, Time.deltaTime * _smoothSpeed);

            _animator.SetFloat("MouseX", _cameraX);

            transform.rotation = Quaternion.Euler(0f, _character.Yaw, 0f);

            _y = Mathf.MoveTowards(_y, _character.IsGrouned ? 0f : 1f, Time.deltaTime * _smoothSpeed);

            _animator.SetFloat("Vertical", _y);

            _horizontalSpeed = Mathf.MoveTowards(_horizontalSpeed, _character.HorizontalSpeed,
                Time.deltaTime * _smoothSpeed);

            var hSpeed = Mathf.InverseLerp(_minSpeed, _maxSpeed, _horizontalSpeed);
            _animator.SetFloat("HorizontalSpeed", hSpeed);
        }
    } 
}

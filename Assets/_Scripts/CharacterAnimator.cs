using UnityEngine;

namespace Faza
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private CharacterInput _input;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _smoothSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _minSpeed;
        [SerializeField] private RuntimeAnimatorController[] _animationSets;
        [SerializeField] private Shooter _shooter;
        [SerializeField] private Health _health;
        [SerializeField] private Transform _bulletOrigin;
        [SerializeField] private Projectile _bulletPrefab;
        [SerializeField] private Transform _healthbarPoint;

        private int _animationSetIndex;
        private float _horizontal;
        private float _vertical;
        private float _cameraX;
        private float _y;
        private float _horizontalSpeed;

        public Transform HealthbarPoint => _healthbarPoint;

        public string IncrementAnimationSet()
        {
            _animationSetIndex++;
            _animationSetIndex %= _animationSets.Length;
            _animator.runtimeAnimatorController = _animationSets[_animationSetIndex];
            return _animator.runtimeAnimatorController.name;
        }

        public void Fire()
        {
            var bullet = Instantiate(_bulletPrefab);
            bullet.transform.position = _bulletOrigin.position;
            var direction = (_shooter.Target.position.WithY(0f) - transform.position.WithY(0f)).normalized;
            bullet.Init(_shooter.Damage, _shooter.Damage, direction);
            _shooter.FinishFire();
        }

        private void Awake()
        {
            if (_health != null)
            {
                _health.OnDeath += Health_OnDeath;
            }
        }

        private void Health_OnDeath()
        {
            _animator.CrossFade("Death", 0.3f);
        }

        private void Update()
        {
            var move = _input.GetMove();

            var horizontal = move.x;
            var vertical = move.z;

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

            if (_shooter != null)
            {
                _animator.SetBool("Shoot", _shooter.IsShooting);
                _shooter.IsShooting = false;
            }
        }
    } 
}

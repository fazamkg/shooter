using System.Collections;
using UnityEngine;

namespace Faza
{
    // todo: use hash for animation states instead of strings
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
        [SerializeField] private MeleeAttack _meleeAttack;

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

        // called from animation event
        public void Fire()
        {
            var bullet = Instantiate(_bulletPrefab);
            bullet.transform.position = _bulletOrigin.position;
            var direction = (_shooter.Target.WithY(0f) - transform.position.WithY(0f)).normalized;
            bullet.Init(_shooter.Damage, _shooter.BulletSpeed, direction, _shooter.Gravity, _shooter.Decay);

            StartCoroutine(FinishFireCoroutine());
        }

        private IEnumerator FinishFireCoroutine()
        {
            yield return new WaitForSeconds(0.75f);
            _shooter.FinishFire();
        }

        // animation event
        public void FinishFire()
        {
            //_shooter.FinishFire();
        }

        // animation event
        public void MeleeAttack()
        {
            var health = _meleeAttack.Target.GetComponent<Health>();
            health.TakeDamage(_meleeAttack.Damage);
        }

        // animation event
        public void FinishMeleeAttack()
        {
            _meleeAttack.FinishAttack();
        }

        private void Awake()
        {
            if (_health != null)
            {
                _health.OnDeath += Health_OnDeath;
                _health.OnHealthChanged += Health_OnHealthChanged;
            }
        }

        private void Health_OnHealthChanged()
        {
            if (_health.CurrentHealth <= 0f) return;
            _animator.CrossFadeInFixedTime("Hit", 0.05f);
        }

        private void Health_OnDeath()
        {
            _animator.CrossFadeInFixedTime("Death", 0.05f);
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

            if (_shooter != null && _shooter.IsShooting)
            {
                _animator.CrossFadeInFixedTime("Attack", 0.05f);
                _shooter.IsShooting = false;
            }

            if (_meleeAttack != null && _meleeAttack.StartAttack)
            {
                _animator.CrossFadeInFixedTime("MeleeAttack", 0.05f);
                _meleeAttack.StartAttack = false;
            }
        }
    } 
}

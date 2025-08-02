using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Animations.Rigging;

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
        
        [SerializeField] private Transform _healthbarPoint;
        [SerializeField] private MeleeAttack _meleeAttack;
        [SerializeField] private Rig[] _rigs;

        [Header("Random Idle")]
        [SerializeField] private bool _switchIdle;
        [SerializeField] private float _switchIdleMinDuration;
        [SerializeField] private float _switchIdleMaxDuration;
        [SerializeField] private float[] _idles;

        private int _animationSetIndex;
        private float _horizontal;
        private float _vertical;
        private float _cameraX;
        private float _y;
        private float _horizontalSpeed;
        private Chest _currentChest;

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
            _shooter.FireBullet();
        }

        // animation event
        public void FinishFire()
        {
            //_shooter.FinishFire();
        }

        // animation event
        public void MeleeAttack()
        {
            var distance = Vector3.Distance(transform.position, _meleeAttack.Target.position);

            if (distance > _meleeAttack.Range + 0.2f) return;

            var health = _meleeAttack.Target.GetComponent<Health>();
            var direction = Quaternion.Euler(0f, _character.Yaw, 0f) * Vector3.forward;
            health.TakeDamage(_meleeAttack.Damage, direction);
        }

        // animation event
        public void FinishMeleeAttack()
        {
            //_meleeAttack.FinishAttack();
        }

        private void Awake()
        {
            if (_rigs != null)
            {
                foreach (var rig in _rigs)
                {
                    rig.weight = 0f;
                }
            }

            if (_health != null)
            {
                _health.OnDeath += Health_OnDeath;
                _health.OnHealthChanged += Health_OnHealthChanged;
            }

            if (_switchIdle)
            {
                _animator.SetFloat("IdleVariant", _idles.GetRandom());
                StartCoroutine(SwitchIdle());
            }
        }

        private IEnumerator SwitchIdle()
        {
            while (true)
            {
                var randomDuration = Random.Range(_switchIdleMinDuration, _switchIdleMaxDuration);
                yield return new WaitForSeconds(randomDuration);

                _animator.DOFloat("IdleVariant", _idles.GetRandom(), 0.3f);
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
            enabled = false;
        }

        private void Update()
        {
            _animator.SetBool("InterruptAttack", false);

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
                _animator.SetFloat("ShootSpeed", _shooter.ShootSpeed);
            }

            if (_shooter != null && _shooter.StartedShooting)
            {
                _animator.CrossFadeInFixedTime("Attack", 0.05f);
                _shooter.StartedShooting = false;
            }

            if (_meleeAttack != null && _meleeAttack.StartAttack)
            {
                _animator.CrossFadeInFixedTime("MeleeAttack", 0.05f);
                _meleeAttack.StartAttack = false;
                _meleeAttack.FinishAttack();
            }

            if (_shooter != null)
            {
                if (_character.HorizontalSpeed > 0.5f && _shooter.BulletFired)
                {
                    _animator.SetBool("InterruptAttack", true);
                }
            }
        }

        public void PlayOutOpenChestAnimation(Chest chest)
        {
            foreach (var rig in _rigs)
            {
                rig.DOWeight(1f, 0.15f);
            }

            _currentChest = chest;
            _character.enabled = false;
            _character.CharacterController.enabled = false;
            _animator.CrossFadeInFixedTime("OpenChest", 0.15f);
        }

        // animation event
        public void TriggerChestOpen()
        {
            _currentChest.PlayOpenChestAnimation();
        }

        // animation event
        public void FinishPlayOutOpenChestAnimation()
        {
            foreach (var rig in _rigs)
            {
                rig.DOWeight(0f, 0.15f);
            }

            _character.enabled = true;
            _character.CharacterController.enabled = true;
        }
    } 
}

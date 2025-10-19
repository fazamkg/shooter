using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Faza
{
    public class CharacterAnimator : MonoBehaviour
    {
        private const float CHEST_ANIMATION_TRANSITION = 0.15f;
        private const float IDLE_TRANSITION = 0.3f;
        private const float HIT_TRANSITION = 0.05f;
        private const float DEATH_TRANSITION = 0.05f;
        private const float ATTACK_TRANSITION = 0.05f;
        private const float MELEE_ATTACK_TRANSITION = 0.05f;
        private const float INTERRUPT_ATTACK_SPEED_CAP = 0.5f;
        private const float MELEE_ATTACK_EXTRA_RANGE = 0.2f;
        private const float SHOOT_SPEED_FACTOR = 0.2428f;

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
        [SerializeField] private AnimationClip _shootClip;

        [Header("Random Idle")]
        [SerializeField] private bool _switchIdle;
        [SerializeField] private float _switchIdleMinDuration;
        [SerializeField] private float _switchIdleMaxDuration;
        [SerializeField] private float[] _idles;

        [Header("Footstep")]
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip[] _stepClips;

        private int _animationSetIndex;
        private float _horizontal;
        private float _vertical;
        private float _cameraX;
        private float _y;
        private float _horizontalSpeed;
        private Chest _currentChest;

        public Transform HealthbarPoint => _healthbarPoint;

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
                _animator.SetFloat(AnimatorKey.IdleVariant, _idles.GetRandom());
                StartCoroutine(SwitchIdleCoroutine());
            }
        }

        private void Update()
        {
            _animator.SetBool(AnimatorKey.InterruptAttack, false);

            var move = _input.GetMove();

            var horizontal = move.x;
            var vertical = move.z;

            _horizontal = Mathf.MoveTowards(_horizontal, horizontal, Time.deltaTime * _smoothSpeed);
            _vertical = Mathf.MoveTowards(_vertical, vertical, Time.deltaTime * _smoothSpeed);

            _animator.SetFloat(AnimatorKey.InputHorizontal, _horizontal);
            _animator.SetFloat(AnimatorKey.InputVertical, _vertical);

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

            _animator.SetFloat(AnimatorKey.MouseX, _cameraX);

            transform.rotation = Quaternion.Euler(0f, _character.Yaw, 0f);

            _y = Mathf.MoveTowards(_y, _character.IsGrouned ? 0f : 1f, Time.deltaTime * _smoothSpeed);

            _animator.SetFloat(AnimatorKey.Vertical, _y);

            _horizontalSpeed = Mathf.MoveTowards(_horizontalSpeed, _character.HorizontalSpeed,
                Time.deltaTime * _smoothSpeed);

            var hSpeed = Mathf.InverseLerp(_minSpeed, _maxSpeed, _horizontalSpeed);
            _animator.SetFloat(AnimatorKey.HorizontalSpeed, hSpeed);

            if (_shooter != null)
            {
                _animator.SetFloat(AnimatorKey.ShootSpeed, _shooter.ShootSpeed);
            }

            if (_shooter != null && _shooter.StartedShooting)
            {
                _animator.CrossFadeInFixedTime(AnimatorKey.Attack, ATTACK_TRANSITION);
                _shooter.StartedShooting = false;
                StartCoroutine(FireCoroutine());
            }

            if (_meleeAttack != null && _meleeAttack.StartAttack)
            {
                _animator.CrossFadeInFixedTime(AnimatorKey.MeleeAttack, MELEE_ATTACK_TRANSITION);
                _meleeAttack.StartAttack = false;
                _meleeAttack.FinishAttack();
            }

            if (_shooter != null)
            {
                if (_character.HorizontalSpeed > INTERRUPT_ATTACK_SPEED_CAP && _shooter.BulletFired)
                {
                    _animator.SetBool(AnimatorKey.InterruptAttack, true);
                }
            }
        }

        public string IncrementAnimationSet()
        {
            _animationSetIndex++;
            _animationSetIndex %= _animationSets.Length;
            _animator.runtimeAnimatorController = _animationSets[_animationSetIndex];
            return _animator.runtimeAnimatorController.name;
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void Fire()
        {
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void FinishFire()
        {
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void MeleeAttack()
        {
            var distance = Vector3.Distance(transform.position, _meleeAttack.Target.position);

            if (distance > _meleeAttack.Range + MELEE_ATTACK_EXTRA_RANGE) return;

            var health = _meleeAttack.Target.GetComponent<Health>();
            var direction = Quaternion.Euler(0f, _character.Yaw, 0f) * Vector3.forward;
            health.TakeDamage(_meleeAttack.Damage, direction);
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void FinishMeleeAttack()
        {
        }

        public void Footstep()
        {
            _source.PlayOneShot(_stepClips.GetRandom());
        }

        public void PlayOutOpenChestAnimation(Chest chest)
        {
            foreach (var rig in _rigs)
            {
                rig.DOWeight(1f, CHEST_ANIMATION_TRANSITION);
            }

            _currentChest = chest;
            _character.enabled = false;
            _character.CharacterController.enabled = false;
            _animator.CrossFadeInFixedTime(AnimatorKey.OpenChest, CHEST_ANIMATION_TRANSITION);
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void TriggerChestOpen()
        {
            _currentChest.PlayOpenChestAnimation();
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void FinishPlayOutOpenChestAnimation()
        {
            foreach (var rig in _rigs)
            {
                rig.DOWeight(0f, CHEST_ANIMATION_TRANSITION);
            }

            _character.enabled = true;
            _character.CharacterController.enabled = true;
        }

        private IEnumerator SwitchIdleCoroutine()
        {
            while (true)
            {
                var randomDuration = Random.Range(_switchIdleMinDuration, _switchIdleMaxDuration);
                yield return new WaitForSeconds(randomDuration);

                _animator.DOFloat(AnimatorKey.IdleVariant, _idles.GetRandom(), IDLE_TRANSITION);
            }
        }

        private void Health_OnHealthChanged()
        {
            if (_health.CurrentHealth <= 0f) return;
            _animator.CrossFadeInFixedTime(AnimatorKey.Hit, HIT_TRANSITION);
        }

        private void Health_OnDeath()
        {
            _animator.CrossFadeInFixedTime(AnimatorKey.Death, DEATH_TRANSITION);
            enabled = false;
        }

        private IEnumerator FireCoroutine()
        {
            var dur = _shootClip.length / _shooter.ShootSpeed * SHOOT_SPEED_FACTOR;
            yield return new WaitForSeconds(dur);
            _shooter.FireBullet();
        }
    } 
}

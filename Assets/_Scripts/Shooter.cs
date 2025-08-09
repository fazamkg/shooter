using UnityEngine;
using System.Collections;

namespace Faza
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private LayerMask _layerMask2;
        [SerializeField] private float _detectionRadius;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _movementDelay;
        [SerializeField] private Character _character;
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _gravity;
        [SerializeField] private float _decay;
        [SerializeField] private ParticleSystem _muzzleEffect;
        [SerializeField] private Transform _bulletOrigin;
        [SerializeField] private Projectile _bulletPrefab;
        [SerializeField] private Health _health;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private Modifier _modDamage;

        private bool _shoot;
        private bool _inCooldown;
        private Collider[] _colliders = new Collider[32];
        private float _shootSpeed = 1f;

        public Vector3 Target { get; private set; }
        public bool StartedShooting { get; set; }
        public float BulletSpeed => _speed;
        public float Damage => _modDamage.Evaluate();
        public float Gravity => _gravity;
        public float Decay => _decay;
        public bool WithinShooting => _shoot;
        public float ShootSpeed => _shootSpeed;
        public bool BulletFired { get; private set; }

        public void AddDamage(float damage)
        {
            _modDamage.AddModifier(ModifierType.Flat, "dmg-flag", damage);
        }

        public void AddProjectileSpeed(float speed)
        {
            _speed += speed;
        }

        public void AddShootingSpeed(float speed)
        {
            _rotationSpeed += speed;
            _cooldown -= speed;
            _cooldown = Mathf.Max(_cooldown, 0f);
            _movementDelay -= speed;
            _movementDelay = Mathf.Max(_movementDelay, 0f);
            _shootSpeed += speed;
        }

        private void Awake()
        {
            _modDamage.Init();
        }

        private void Update()
        {
            if (_character.enabled == false) return;

            if (_shoot) return;
            if (_inCooldown) return;

            if (_health.IsDead) return;

            if (_input.GetRawMove().sqrMagnitude > 0.1f) return;
            if (_character.HorizontalSpeed > 1f) return;

            var amount = Physics.OverlapSphereNonAlloc
                (transform.position, _detectionRadius, _colliders, _layerMask);

            var minHealth = float.MaxValue;
            var minDistance = float.MaxValue;
            EnemyInput closest = null;

            if (amount == 0) return;

            for (var i = 0; i < amount; i++)
            {
                var collider = _colliders[i];

                var enemy = collider.GetComponent<EnemyInput>();
                if (enemy == false) continue;

                var vector = (enemy.transform.position - transform.position);

                var ray = new Ray(transform.position.DeltaY(0.5f), vector.normalized);
                var hit = Physics.Raycast(ray, out var hitInfo, 100f, _layerMask2);
                if (hit == false) continue;

                if (hitInfo.collider.GetComponent<EnemyInput>() == false) continue;

                var distance = vector.magnitude;
                var health = enemy.Health.CurrentHealth;

                if ((health - minHealth).Abs() < 0.01f)
                {
                    if (distance < minDistance)
                    {
                        closest = enemy;
                        minHealth = health;
                        minDistance = distance;
                    }
                }
                else if (health < minHealth)
                {
                    closest = enemy;
                    minHealth = health;
                    minDistance = distance;
                }
            }

            if (closest == null) return;

            var target = closest.transform.position;

            _shoot = true;

            Target = target;

            StartCoroutine(ShootCoroutine(target));
        }

        private IEnumerator ShootCoroutine(Vector3 target)
        {
            BulletFired = false;

            yield return _character.RotateTowardsCoroutine(target, _rotationSpeed);

            StartedShooting = true;

            _inCooldown = true;
            _shoot = true;

            StartCoroutine(Cooldown());
            StartCoroutine(FinishFireCoroutine());
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(_cooldown);
            _inCooldown = false;
        }

        private IEnumerator FinishFireCoroutine()
        {
            yield return new WaitForSeconds(_movementDelay);
            _shoot = false;
        }

        public void FireBullet()
        {
            _muzzleEffect.Play();

            var bullet = Instantiate(_bulletPrefab);
            bullet.transform.position = _bulletOrigin.position;
            var direction = (Target.WithY(0f) - transform.position.WithY(0f)).normalized;
            bullet.Init(Damage, BulletSpeed, direction, Gravity, Decay);

            BulletFired = true;
        }
    } 
}

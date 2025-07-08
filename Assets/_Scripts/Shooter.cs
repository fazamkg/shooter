using UnityEngine;
using System.Collections;

namespace Faza
{
    public class Shooter : MonoBehaviour
    {
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

        private bool _shoot;
        private bool _inCooldown;
        private Collider[] _colliders = new Collider[32];

        public Vector3 Target { get; private set; }
        public bool StartedShooting { get; set; }
        public float BulletSpeed => _speed;
        public float Damage => _damage;
        public float Gravity => _gravity;
        public float Decay => _decay;
        public bool WithinShooting => _shoot;

        private void Update()
        {
            if (_shoot) return;
            if (_inCooldown) return;

            if (_character.HorizontalSpeed > 1f) return;

            var amount = Physics.OverlapSphereNonAlloc(transform.position, _detectionRadius, _colliders);
            var minDistance = float.MaxValue;
            EnemyInput closest = null;

            if (amount == 0) return;

            for (var i = 0; i < amount; i++)
            {
                var collider = _colliders[i];

                var enemy = collider.GetComponent<EnemyInput>();
                if (enemy == false) continue;

                var distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < minDistance)
                {
                    closest = enemy;
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
        }
    } 
}

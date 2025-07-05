using UnityEngine;
using System.Collections;

namespace Faza
{
    public class Shooter : MonoBehaviour
    {
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

        public Vector3 Target { get; private set; }
        public bool IsShooting { get; set; }
        public float BulletSpeed => _speed;
        public float Damage => _damage;
        public float Gravity => _gravity;
        public float Decay => _decay;

        private void Update()
        {
            if (_shoot) return;

            if (ShootingTapArea.IsDown == false) return;

            if (_character.HorizontalSpeed > 1f) return;

            var mouse = Input.mousePosition;
            mouse.z = 0f;
            var playerScreen = Camera.main.WorldToScreenPoint(transform.position);
            playerScreen.z = 0f;
            var screenDirection = (Input.mousePosition - playerScreen).normalized;

            var target = transform.position + new Vector3(screenDirection.x, 0f, screenDirection.y);

            _shoot = true;

            Target = target;

            StartCoroutine(ShootCoroutine(target));
        }

        private IEnumerator ShootCoroutine(Vector3 target)
        {
            yield return _character.RotateTowardsCoroutine(target, _rotationSpeed);

            IsShooting = true;
        }

        public void FinishFire()
        {
            _shoot = false;
        }

        public void FireBullet()
        {
            _muzzleEffect.Play();

            var bullet = Instantiate(_bulletPrefab);
            bullet.transform.position = _bulletOrigin.position;
            var direction = (Target.WithY(0f) - transform.position.WithY(0f)).normalized;
            bullet.Init(Damage, BulletSpeed, direction, Gravity, Decay);

            StartCoroutine(FinishFireCoroutine());
        }

        private IEnumerator FinishFireCoroutine()
        {
            yield return new WaitForSeconds(0.75f);
            FinishFire();
        }
    } 
}

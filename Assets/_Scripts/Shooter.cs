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

        private bool _shoot;

        public Vector3 Target { get; private set; }
        public bool IsShooting { get; set; }
        public float BulletSpeed => _speed;
        public float Damage => _damage;

        private void Awake()
        {
            ShootingTapArea.OnTap += ShootingTapArea_OnTap;
        }

        private void OnDestroy()
        {
            ShootingTapArea.OnTap -= ShootingTapArea_OnTap;
        }

        private void ShootingTapArea_OnTap()
        {
            if (_shoot) return;

            if (Input.GetMouseButtonDown(0) == false) return;

            if (_character.HorizontalSpeed > 1f) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var hit = Physics.Raycast(ray, out var info, 100f, ~0);
            if (hit == false) return;

            _shoot = true;

            Target = info.point;

            StartCoroutine(ShootCoroutine(info.point));
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
    } 
}

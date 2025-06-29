using UnityEngine;
using System.Collections;

namespace Faza
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;

        private bool _shoot;

        public bool IsShooting { get; set; }
        public float BulletSpeed => _speed;
        public float Damage => _damage;
        public Transform Target { get; set; }

        private void Update()
        {
            if (_shoot) return;

            if (Input.GetMouseButtonDown(0) == false) return;

            if (_character.HorizontalSpeed > 0.05f) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var hit = Physics.Raycast(ray, out var info, 100f, ~0);
            if (hit == false) return;

            var enemy = info.collider.GetComponent<EnemyInput>();
            if (enemy == false) return;

            _shoot = true;

            Target = enemy.transform;

            StartCoroutine(ShootCoroutine(enemy.transform.position));
        }

        private IEnumerator ShootCoroutine(Vector3 target)
        {
            yield return _character.RotateTowardsCoroutine(target);

            IsShooting = true;
        }

        public void FinishFire()
        {
            _shoot = false;
        }
    } 
}

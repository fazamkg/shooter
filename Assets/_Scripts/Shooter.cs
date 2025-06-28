using UnityEngine;

namespace Faza
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private Character _character;

        public bool IsShooting { get; set; }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) == false) return;

            if (_character.HorizontalSpeed > 0.05f) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var hit = Physics.Raycast(ray, out var info, 100f, ~0);
            if (hit == false) return;

            var enemy = info.collider.GetComponent<EnemyInput>();
            if (enemy == false) return;

            _character.RotateTowards(enemy.transform.position);

            IsShooting = true;

            enemy.GetComponent<Health>().TakeDamage(40f);
        }
    } 
}

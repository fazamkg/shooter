using UnityEngine;

namespace Faza
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private ParticleSystem _hitEffect;

        private float _damage;
        private float _speed;
        private Vector3 _direction;

        public void Init(float damage, float speed, Vector3 direction)
        {
            _damage = damage;
            _speed = speed;
            _direction = direction;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void Update()
        {
            transform.position += _speed * Time.deltaTime * _direction;
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<EnemyInput>();
            if (enemy == false) return;

            var health = other.GetComponent<Health>();
            health.TakeDamage(_damage);

            enabled = false;
            _renderer.enabled = false;
            _hitEffect.Play();
        }
    } 
}

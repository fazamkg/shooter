using UnityEngine;

namespace Faza
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private ParticleSystem _hitEffect;
        [SerializeField] private Collider _collider;

        private float _damage;
        private float _speed;
        private Vector3 _direction;
        private float _verticalSpeed;
        private float _gravity;

        public void Init(float damage, float speed, Vector3 direction, float gravity)
        {
            _damage = damage;
            _speed = speed;
            _direction = direction;
            _gravity = gravity;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void Update()
        {
            _verticalSpeed -= _gravity * Time.deltaTime;

            transform.position += _speed * Time.deltaTime * _direction;
            transform.position += _verticalSpeed * Time.deltaTime * Vector3.up;
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<EnemyInput>();
            if (enemy == false) return;

            var health = other.GetComponent<Health>();
            health.TakeDamage(_damage);

            enabled = false;
            _renderer.enabled = false;
            _collider.enabled = false;
            _hitEffect.Play();
        }
    } 
}

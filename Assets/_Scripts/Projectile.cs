using UnityEngine;

namespace Faza
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private ParticleSystem _hitEffect;
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private GameObject _bulletHolePrefab;
        [SerializeField] private GameObject _critGlow;
        [SerializeField] private TrailRenderer _trail;

        private float _damage;
        private float _speed;
        private Vector3 _direction;
        private Transform _target;
        private float _verticalSpeed;
        private float _gravity;
        private float _decay;

        private Vector3 _hitPosition;
        private Vector3 _hitNormal;

        public void Init(float damage, float speed, Vector3 direction, Transform target, float gravity, float decay)
        {
            _damage = damage;
            _speed = speed;
            _direction = direction;
            _target = target;
            _gravity = gravity;
            _decay = decay;
            transform.rotation = Quaternion.LookRotation(direction);
            _rigidbody.position = transform.position;

            _trail.enabled = true;
            _critGlow.SetActive(false);
        }

        public void ActivateCritGlow()
        {
            _trail.enabled = false;
            _critGlow.SetActive(true);
        }

        private void FixedUpdate()
        {
            _verticalSpeed -= _gravity * Time.fixedDeltaTime;
            _speed -= _decay * Time.fixedDeltaTime;

            var position = _rigidbody.position;

            var direction = (_target.position.WithY(0f) - transform.position.WithY(0f)).normalized;

            position += _speed * Time.fixedDeltaTime * direction;
            position += _verticalSpeed * Time.fixedDeltaTime * Vector3.up;

            _rigidbody.MovePosition(position);

            var vector = position - _rigidbody.position;
            var hit = _rigidbody.SweepTest(vector.normalized, out var info, vector.magnitude);
            if (hit)
            {
                _hitPosition = info.point;
                _hitNormal = info.normal;

                _rigidbody.MovePosition(info.point);
            }
            else
            {
                _rigidbody.MovePosition(position);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<EnemyInput>();
            if (enemy != false)
            {
                var health = other.GetComponent<Health>();
                health.TakeDamage(_damage, _direction);
            }

            if (other.GetComponent<PlayerInput>() == false)
            {
                enabled = false;
                _renderer.enabled = false;
                _collider.enabled = false;
                _hitEffect.Play();
                _critGlow.SetActive(false);


                if (enemy == false)
                {
                    var pos = _hitPosition + _hitNormal * 0.1f;
                    var rot = Quaternion.LookRotation(-_hitNormal);
                    var bulletHole = Instantiate(_bulletHolePrefab, pos, rot);
                    bulletHole.transform.Rotate(0f, 0f, Random.Range(0f, 360f), Space.Self);
                }
            }
        }
    } 
}

using UnityEngine;
using System.Collections;

namespace Faza
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _speed;
        [SerializeField] private Collider _collider;

        private Rigidbody _target;
        private bool _picked;
        private PlayerInput _player;

        private IEnumerator ActivateCollider()
        {
            yield return new WaitForSeconds(1f);
            _collider.enabled = true;
        }

        public void SetTarget(Rigidbody rb)
        {
            _target = rb;
            StartCoroutine(ActivateCollider());
        }

        public void AutoMagnet()
        {
            _picked = true;
            _player = PlayerInput.Instance;
        }

        private void LateUpdate()
        {
            if (_picked == false)
            {
                var pos = _target.position + _target.rotation * _target.centerOfMass;
                transform.position = pos + Vector3.up;
            }
            else
            {
                var target = _player.transform.position.DeltaY(1f);
                var dist = Vector3.Distance(transform.position, target);
                var speed = 1f / dist * Time.deltaTime * 8f;

                if (dist < 0.15f)
                {
                    Currency.AddCoins(1f, transform.position);
                    gameObject.SetActive(false);
                }

                transform.position = Vector3.MoveTowards(transform.position, target, speed);

                transform.localScale = Vector3.one * Mathf.Min(dist * 2f, 1f);
            }
            
            _transform.Rotate(0f, Time.deltaTime * _speed, 0f, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_picked) return;

            var player = other.GetComponent<PlayerInput>();
            if (player == false) return;

            _picked = true;
            _player = player;
        }
    } 
}

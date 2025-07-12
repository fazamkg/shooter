using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _speed;

        private Rigidbody _target;
        private bool _picked;
        private PlayerInput _player;

        public void SetTarget(Rigidbody rb)
        {
            _target = rb;
        }

        private void Update()
        {
            if (_picked == false)
            {
                transform.position = _target.worldCenterOfMass + Vector3.up;
            }
            else
            {
                var target = _player.transform.position.DeltaY(1f);
                var dist = Vector3.Distance(transform.position, target);
                var speed = dist * Time.deltaTime * 8f;

                if (dist < 0.15f)
                {
                    Currency.Coins++;
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

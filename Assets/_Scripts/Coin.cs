using UnityEngine;
using System.Collections;

namespace Faza
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private Transform _toMove;
        [SerializeField] private Transform _toRotate;
        [SerializeField] private float _speed;
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;

        private bool _picked;
        private PlayerInput _player;

        private bool _initialDistanceSet;
        private float _initialDistance;

        public Rigidbody Rigidbody => _rigidbody;

        private void Awake()
        {
            _collider.transform.SetParent(null);
        }

        private IEnumerator ActivateCollider()
        {
            yield return new WaitForSeconds(1f);
            _collider.enabled = true;
            yield return new WaitForSeconds(0.3f);
            AutoMagnet();
        }

        public void ActivateColliderDelayed()
        {
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
                var pos = _rigidbody.position + _rigidbody.rotation * _rigidbody.centerOfMass;
                _toMove.position = pos + Vector3.up * 0.25f;
            }
            else
            {
                var target = _player.transform.position.DeltaY(1f);
                var dist = Vector3.Distance(_toMove.position, target);
                if (_initialDistanceSet == false)
                {
                    _initialDistanceSet = true;
                    _initialDistance = dist;
                }

                var speed = _initialDistance / dist * Time.deltaTime * 8f;

                if (dist < 0.3f)
                {
                    Currency.AddCoins(1f, _toMove.position);
                    gameObject.SetActive(false);
                    _collider.gameObject.SetActive(false);
                }

                _toMove.position = Vector3.MoveTowards(_toMove.position, target, speed);

                _toMove.localScale = Vector3.one * Mathf.Min(dist * 2f, 1f);
            }
            
            _toRotate.Rotate(0f, Time.deltaTime * _speed, 0f, Space.World);
        }

        public void MyOnTriggerEnter(Collider other)
        {
            if (_picked) return;

            var player = other.GetComponent<PlayerInput>();
            if (player == false) return;

            _picked = true;
            _player = player;
        }
    } 
}

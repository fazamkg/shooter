using UnityEngine;

namespace Faza
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _speed;

        private Rigidbody _target;

        public void SetTarget(Rigidbody rb)
        {
            _target = rb;
        }

        private void Update()
        {
            transform.position = _target.worldCenterOfMass + Vector3.up;

            _transform.Rotate(0f, Time.deltaTime * _speed, 0f, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerInput>();
            if (player == false) return;

            Currency.Coins++;

            gameObject.SetActive(false);
        }
    } 
}

using UnityEngine;

namespace Faza
{
    public class Fan : MonoBehaviour
    {
        [SerializeField] private Transform _toRotate;
        [SerializeField] private float _speed;
        [SerializeField] private float _strength;

        private void Update()
        {
            _toRotate.Rotate(0f, Time.deltaTime * _speed, 0f, Space.Self);
        }

        private void OnTriggerStay(Collider other)
        {
            var character = other.GetComponent<Character>();
            if (character == false) return;

            var direction = transform.up;
            var distance = Vector3.Distance(transform.position, character.transform.position);
            var oppDist = 1f / distance;

            var accel = oppDist * _strength * Time.fixedDeltaTime * direction;

            character.AddVelocity(accel);
        }
    } 
}

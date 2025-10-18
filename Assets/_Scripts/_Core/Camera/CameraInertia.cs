using UnityEngine;

namespace Faza
{
    public class CameraInertia : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;

        public Vector3 Delta { get; private set; }

        private void LateUpdate()
        {
            var target = _target.position;
            var distance = Vector3.Distance(transform.position, target);
            var speed = _speed * distance * Time.deltaTime;

            var lastPos = transform.position;
            var newPos = Vector3.MoveTowards(transform.position, target, speed);
            Delta = newPos - lastPos;

            transform.position = newPos;
        }
    } 
}

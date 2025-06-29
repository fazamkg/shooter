using UnityEngine;

namespace Faza
{
    public class CameraInertia : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;

        private void LateUpdate()
        {
            var target = _target.position;
            var distance = Vector3.Distance(transform.position, target);
            var speed = _speed * distance * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target, speed);
        }
    } 
}

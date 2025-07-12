using UnityEngine;

namespace Faza
{
    public class PlayerCameraTarget : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float _speed;
        [SerializeField] private float _radius;
        [SerializeField] private float _distance;

        private Vector3 _originalPosition;

        private void Awake()
        {
            _originalPosition = transform.localPosition;
        }

        private void Update()
        {
            var move = _playerInput.GetRawMove() * _radius;
            var target = _originalPosition + move + Vector3.up * move.magnitude * _distance;
            var distance = Vector3.Distance(transform.localPosition, target);
            var speed = _speed * distance * Time.deltaTime;

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed);
        }
    } 
}

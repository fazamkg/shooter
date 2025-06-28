using UnityEngine;
using UnityEngine.EventSystems;

namespace Faza
{
    public class JoystickView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _radius;
        [SerializeField] private string _name;

        private bool _isUpdating;
        private Vector3 _originalPosition;

        private void Awake()
        {
            _originalPosition = transform.position;
        }

        private void Update()
        {
            if (_isUpdating == false) return;

            var mousePos = Input.mousePosition;
            var vector = mousePos - _originalPosition;
            var clamped = Vector3.ClampMagnitude(vector, _radius);

            Joystick.SetInput(_name, clamped / _radius);

            transform.position = _originalPosition + clamped;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isUpdating = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isUpdating = false;

            Joystick.SetInput(_name, Vector3.zero);

            transform.position = _originalPosition;
        }
    } 
}

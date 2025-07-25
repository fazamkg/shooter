using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Faza
{
    public class JoystickView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _radius;
        [SerializeField] private string _name;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _tweenDuration;

        private bool _isUpdating;

        private void Update()
        {
            if (_isUpdating == false) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle
                ((RectTransform)_rectTransform.parent,
                Input.mousePosition,
                null,
                out var result);

            var clamped = Vector3.ClampMagnitude(result, _radius);

            Joystick.SetInput(_name, clamped / _radius);

            _rectTransform.anchoredPosition = Vector3.MoveTowards
                (_rectTransform.anchoredPosition, clamped, Time.deltaTime * _speed);
        }

        public void OnDown()
        {
            _isUpdating = true;
            Joystick.SetInput(_name, Vector3.zero);
        }

        public void OnUp()
        {
            _isUpdating = false;

            Joystick.SetInput(_name, Vector3.zero);

            _rectTransform.DOAnchorPos(Vector2.zero, _tweenDuration).SetEase(Ease.InOutCirc);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnUp();
        }
    } 
}

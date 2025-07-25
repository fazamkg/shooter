using UnityEngine;
using UnityEngine.EventSystems;

namespace Faza
{
    public class JoystickTapArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _joystickPrefab;

        private RectTransform _current;

        public void OnPointerDown(PointerEventData eventData)
        {
            _current = Instantiate(_joystickPrefab, transform);
            _current.position = eventData.position;

            _current.GetComponentInChildren<JoystickView>().OnDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_current != null)
            {
                _current.GetComponentInChildren<JoystickView>().OnDown();
                Destroy(_current.gameObject);
            }
        }
    }
}

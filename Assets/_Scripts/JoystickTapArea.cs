using UnityEngine;
using UnityEngine.EventSystems;

namespace Faza
{
    public class JoystickTapArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _joystickPrefab;

        private RectTransform _current;

        private void Update()
        {
            if (Input.GetMouseButton(0) == false && _current != null)
            {
                _current.GetComponentInChildren<JoystickView>().OnDown();
                Destroy(_current.gameObject);
                _current = null;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_current != null) return;

            _current = Instantiate(_joystickPrefab, transform);
            _current.position = eventData.position;

            _current.GetComponentInChildren<JoystickView>().OnDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_current == null) return;

            _current.GetComponentInChildren<JoystickView>().OnDown();
            Destroy(_current.gameObject);
            _current = null;
        }
    }
}

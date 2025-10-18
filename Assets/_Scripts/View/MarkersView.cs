using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    [DefaultExecutionOrder(-1)]
    public class MarkersView : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private RectTransform _markerPrefab;
        [SerializeField] private float _topMargin;
        [SerializeField] private float _leftMargin;
        [SerializeField] private float _rightMargin;
        [SerializeField] private float _bottomMargin;

        private Dictionary<EnemyInput, RectTransform> _markers = new();
        private Canvas _canvas;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _canvas = GetComponentInParent<Canvas>();

            Health.OnHealthCreated += Health_OnHealthCreated;
            Health.OnHealthDestroyed += Health_OnHealthDestroyed;
        }

        private void Update()
        {
            foreach (var thing in _markers)
            {
                var enemy = thing.Key;
                var marker = thing.Value;

                var screen = _camera.WorldToScreenPoint(enemy.transform.position);
                if (screen.z < 0f)
                {
                    screen *= -1f;
                }
                screen.z = 0f;

                var left = _leftMargin * _canvas.scaleFactor;
                var right = Screen.width - _rightMargin * _canvas.scaleFactor;
                var bottom = _bottomMargin * _canvas.scaleFactor;
                var top = Screen.height - _topMargin * _canvas.scaleFactor;

                screen.x = Mathf.Clamp(screen.x, left, right);
                screen.y = Mathf.Clamp(screen.y, bottom, top);

                if (screen.x > left && screen.x < right &&
                    screen.y > bottom && screen.y < top)
                {
                    marker.gameObject.SetActive(false);
                }
                else
                {
                    marker.gameObject.SetActive(true);
                }

                var screenPosOfPlayer = _camera.WorldToScreenPoint
                    (PlayerInput.Instance.transform.position);
                var direction = (screenPosOfPlayer - screen).normalized;

                marker.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                marker.position = screen;
            }
        }

        private void Health_OnHealthDestroyed(Health health)
        {
            var enemy = health.GetComponent<EnemyInput>();
            if (enemy == false) return;

            var marker = _markers[enemy];
            Destroy(marker.gameObject);
            _markers.Remove(enemy);
        }

        private void Health_OnHealthCreated(Health health)
        {
            var enemy = health.GetComponent<EnemyInput>();
            if (enemy == false) return;

            var marker = Instantiate(_markerPrefab, _parent);
            marker.gameObject.name = enemy.gameObject.name;
            _markers[enemy] = marker;
        }
    } 
}

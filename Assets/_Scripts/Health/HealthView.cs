using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private RectTransform _fill;
        [SerializeField] private float _width = 230f;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector3 _screenOffset;
        [SerializeField] private Transform _shakeTransform;
        [SerializeField] private float _shakeDuration;
        [SerializeField] private Vector3 _shakeStrength;
        [SerializeField] private int _shakeVibrato;
        [SerializeField] private RectTransform _gloss;

        private Transform _target;
        private Health _health;

        private Camera _camera;

        public void Init(Health health)
        {
            _health = health;
            _target = health.HealthbarPoint;
            InstantUpdateView(health.CurrentHealth, health.MaxHealth);

            health.OnHealthChanged += Health_OnHealthChanged;
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            if (_camera == null)
            {
                _camera = Camera.main;
            }

            var pos = _target.position + _offset;
            transform.position = _camera.WorldToScreenPoint(pos) + _screenOffset;
        }

        public void InstantUpdateView(float current, float max)
        {
            var x = current / max * _width;

            _fill.SetAnchorPosX(x);
            _gloss.SetWidth(x - 2.1f * 2f);
        }

        public Tween UpdateView(float current, float max)
        {
            var x = current / max * _width;

            var seq = DOTween.Sequence();
            seq.Append(_fill.DOAnchorPosX(x, 0.15f).SetEase(Ease.InOutCirc));

            var temp = _gloss.sizeDelta;
            temp.x = x - 2.1f * 2f;
            seq.Join(_gloss.DOSizeDelta(temp, 0.15f).SetEase(Ease.InOutCirc));

            return seq;
        }

        public Tween TweenDisappear()
        {
            return transform.DOScale(0f, 0.3f).SetEase(Ease.InBack)
                .OnComplete(() => Destroy(gameObject));
        }

        private void Health_OnHealthChanged()
        {
            _shakeTransform.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato);

            UpdateView(_health.CurrentHealth, _health.MaxHealth);
        }
    }
}
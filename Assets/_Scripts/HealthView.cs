using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private RectTransform _fill;
        [SerializeField] private float _width = 230f;
        [SerializeField] private Vector3 _offset;

        private Transform _target;
        private Health _health;

        public void Init(Health health)
        {
            _health = health;
            _target = health.HealthbarPoint;
            InstantUpdateView(health.CurrentHealth, health.MaxHealth);

            health.OnHealthChanged += Health_OnHealthChanged;
        }

        private void Health_OnHealthChanged()
        {
            UpdateView(_health.CurrentHealth, _health.MaxHealth);
        }

        public void InstantUpdateView(float current, float max)
        {
            _fill.SetAnchorX(current / max * _width);
        }

        public Tween UpdateView(float current, float max)
        {
            var percent = current / max;

            return _fill.DOAnchorPosX(percent * _width, 0.3f).SetEase(Ease.InOutCirc);
        }

        public Tween TweenDisappear()
        {
            return transform.DOScale(0f, 0.3f).SetEase(Ease.InBack)
                .OnComplete(() => Destroy(gameObject));
        }

        private void LateUpdate()
        {
            var pos = _target.position + _offset;
            transform.position = Camera.main.WorldToScreenPoint(pos);
        }
    }
}
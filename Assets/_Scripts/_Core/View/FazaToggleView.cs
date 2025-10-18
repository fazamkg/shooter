using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace Faza
{
    public class FazaToggleView : MonoBehaviour
    {
        private const float WIDTH = 30f;
        private const float TWEEN_DURATION = 0.3f;
        private const Ease TWEEN_EASE = Ease.InOutCirc;

        public event Action OnToggle;

        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _point;
        [SerializeField] private RectTransform _itself;
        [SerializeField] private Image _fill;

        public bool IsOn { get; private set; }

        public void Init(bool isOn)
        {
            IsOn = isOn;

            _point.SetAnchorPosX(IsOn ? (_itself.sizeDelta.x - WIDTH) : WIDTH);

            _fill.fillAmount = IsOn ? 1f : 0f;
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            IsOn = !IsOn;

            _point.DOKill();
            _fill.DOKill();
            _point.DOAnchorPosX(IsOn ? (_itself.sizeDelta.x - WIDTH) : WIDTH, TWEEN_DURATION)
                .SetEase(TWEEN_EASE);
            _fill.DOFillAmount(IsOn ? 1f : 0f, TWEEN_DURATION)
                .SetEase(TWEEN_EASE);

            OnToggle?.Invoke();
        }
    } 
}

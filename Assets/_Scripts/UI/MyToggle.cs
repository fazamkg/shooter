using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace Faza
{
    public class MyToggle : MonoBehaviour
    {
        public event Action OnToggle;

        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _point;
        [SerializeField] private RectTransform _itself;
        [SerializeField] private Image _fill;

        public bool IsOn { get; private set; }

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        public void Init(bool isOn)
        {
            IsOn = isOn;

            _point.SetAnchorPosX(IsOn ? (_itself.sizeDelta.x - 30f) : (0f + 30f));
            _fill.fillAmount = IsOn ? 1f : 0f;
        }

        private void OnClick()
        {
            IsOn = !IsOn;

            _point.DOKill();
            _fill.DOKill();
            _point.DOAnchorPosX(IsOn ? (_itself.sizeDelta.x - 30f) : (0f + 30f), 0.3f)
                .SetEase(Ease.InOutCirc);
            _fill.DOFillAmount(IsOn ? 1f : 0f, 0.3f)
                .SetEase(Ease.InOutCirc);

            OnToggle?.Invoke();
        }
    } 
}

using UnityEngine;
using DG.Tweening;
using System;

namespace Faza
{
    public class UpgradeScreenView : MonoBehaviour
    {
        public event Action OnClosed;

        private const float FADE_DURATION = 0.5f;
        private const Ease FADE_EASE = Ease.InOutCirc;
        private const float TITLE_DURATION = 0.3f;
        private const Ease TITLE_EASE = Ease.OutBack;
        private const float WAIT = 0.3f;
        private const float BUTTON_DURATION = 0.3f;
        private const Ease BUTTON_EASE = Ease.InOutBack;
        private const float DISAPPEAR_FADE_DURATION = 0.5f;
        private const Ease DISAPPEAR_FADE_EASE = Ease.InOutCirc;
        private const float DISAPPEAR_TITLE_DURATION = 0.3f;
        private const Ease DISAPPEAR_TITLE_EASE = Ease.InBack;
        private const float DISAPPEAR_BUTTON_DURATION = 0.3f;
        private const Ease DISAPPEAR_BUTTON_EASE = Ease.InOutBack;

        [SerializeField] private RectTransform _title;
        [SerializeField] private FazaButtonView _nextButton;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Transform _upgradeParent;
        [SerializeField] private UpgradeView _upgradeViewPrefab;

        private bool _appeared;

        public void Init()
        {
            _group.alpha = 0f;
            _group.blocksRaycasts = false;
            Console.Log("Block raycasts = false from Init()");

            _nextButton.OnUp += NextButton_OnUp;
        }

        public void Appear(UpgradeGroupData[] groups)
        {
            _group.blocksRaycasts = true;
            Console.Log("Block raycasts = true from Appear()");

            _title.localScale = Vector3.zero;
            _nextButton.transform.localScale = Vector3.zero;

            var seq = DOTween.Sequence();

            seq.Append(_group.DOFade(1f, FADE_DURATION).SetEase(FADE_EASE));
            seq.Append(_title.DOScale(1f, TITLE_DURATION).SetEase(TITLE_EASE));
            seq.AppendInterval(WAIT);
            seq.Append(_nextButton.transform.DOScale(1f, BUTTON_DURATION).SetEase(BUTTON_EASE));
            seq.OnComplete(() => _appeared = true);
            seq.SetEase(Ease.Linear);

            foreach (var group in groups)
            {
                var view = Instantiate(_upgradeViewPrefab, _upgradeParent);
                view.Init(group);
            }
        }

        private void NextButton_OnUp()
        {
            if (_appeared == false) return;

            _group.blocksRaycasts = false;
            Console.Log("Block raycasts = false from NextButton_OnUp()");

            var seq = DOTween.Sequence();

            seq.Append(_group.DOFade(0f, DISAPPEAR_FADE_DURATION).SetEase(DISAPPEAR_FADE_EASE));
            seq.Append(_title.DOScale(0f, DISAPPEAR_TITLE_DURATION).SetEase(DISAPPEAR_TITLE_EASE));
            seq.Append(_nextButton.transform.DOScale(0f, DISAPPEAR_BUTTON_DURATION).SetEase(DISAPPEAR_BUTTON_EASE));
            seq.SetEase(Ease.Linear);

            OnClosed?.Invoke();
        }
    } 
}

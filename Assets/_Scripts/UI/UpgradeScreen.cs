using UnityEngine;
using DG.Tweening;
using System;

namespace Faza
{
    public class UpgradeScreen : MonoBehaviour
    {
        public event Action OnClosed;

        [SerializeField] private RectTransform _title;
        [SerializeField] private MyButton _nextButton;
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

        private void NextButton_OnUp()
        {
            if (_appeared == false) return;

            _group.blocksRaycasts = false;
            Console.Log("Block raycasts = false from NextButton_OnUp()");

            var seq = DOTween.Sequence();

            seq.Append(_group.DOFade(0f, 0.5f).SetEase(Ease.InOutCirc));
            seq.Append(_title.DOScale(0f, 0.3f).SetEase(Ease.InBack));
            seq.Append(_nextButton.transform.DOScale(0f, 0.3f).SetEase(Ease.InOutBack));
            seq.SetEase(Ease.Linear);

            OnClosed?.Invoke();
        }

        public void Appear(UpgradeGroupData[] groups)
        {
            _group.blocksRaycasts = true;
            Console.Log("Block raycasts = true from Appear()");

            _title.localScale = Vector3.zero;
            _nextButton.transform.localScale = Vector3.zero;

            var seq = DOTween.Sequence();

            seq.Append(_group.DOFade(1f, 0.5f).SetEase(Ease.InOutCirc));
            seq.Append(_title.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
            seq.AppendInterval(0.3f);
            seq.Append(_nextButton.transform.DOScale(1f, 0.3f).SetEase(Ease.InOutBack));
            seq.OnComplete(() => _appeared = true);
            seq.SetEase(Ease.Linear);

            foreach (var group in groups)
            {
                var view = Instantiate(_upgradeViewPrefab, _upgradeParent);
                view.Init(group);
            }
        }
    } 
}

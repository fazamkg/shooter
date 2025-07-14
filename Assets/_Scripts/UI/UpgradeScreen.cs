using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class UpgradeScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform _title;
        [SerializeField] private MyButton _nextButton;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Transform _upgradeParent;
        [SerializeField] private UpgradeView _upgradeViewPrefab;

        private bool _appeared;

        private void Awake()
        {
            _group.alpha = 0f;
            _group.blocksRaycasts = false;

            _nextButton.OnUp += NextButton_OnUp;
        }

        private void NextButton_OnUp()
        {
            if (_appeared == false) return;

            _group.blocksRaycasts = false;

            var seq = DOTween.Sequence();

            seq.Append(_group.DOFade(0f, 0.5f).SetEase(Ease.InOutCirc));
            seq.Append(_title.DOScale(0f, 0.3f).SetEase(Ease.InBack));
            seq.Append(_nextButton.transform.DOScale(0f, 0.3f).SetEase(Ease.InOutBack));
            seq.SetEase(Ease.Linear);
        }

        public void Appear(UpgradeGroupData[] groups)
        {
            _group.blocksRaycasts = true;
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

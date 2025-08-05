using DG.Tweening;
using UnityEngine;

namespace Faza
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform _title;
        [SerializeField] private RectTransform _timerBefore;
        [SerializeField] private RectTransform _timer;
        [SerializeField] private MyButton _nextButton;
        [SerializeField] private CanvasGroup _group;

        private void Awake()
        {
            _group.alpha = 0f;
            _group.blocksRaycasts = false;

            _nextButton.OnUp += NextButton_OnUp;
        }

        private void NextButton_OnUp()
        {
            LevelManager.Instance.LoadLevelFromSave();
        }

        public void Appear(float speed = 1f)
        {
            _group.blocksRaycasts = true;
            _title.localScale = Vector3.zero;
            _nextButton.transform.localScale = Vector3.zero;
            _timerBefore.localScale = Vector3.zero;
            _timer.localScale = Vector3.zero;

            var seq = DOTween.Sequence();

            seq.AppendInterval(1.5f / speed);
            seq.Append(_group.DOFade(1f, 0.5f / speed).SetEase(Ease.InOutCirc));
            seq.Append(_title.DOScale(1f, 0.3f / speed).SetEase(Ease.OutBack));
            seq.AppendInterval(0.3f / speed);
            seq.Append(_timerBefore.DOScale(1f, 0.3f / speed).SetEase(Ease.OutBack));
            seq.AppendInterval(0.3f / speed);
            seq.Append(_timer.DOScale(1f, 0.3f / speed).SetEase(Ease.OutBack));
            seq.AppendInterval(0.3f / speed);
            seq.Append(_nextButton.transform.DOScale(1f, 0.3f / speed).SetEase(Ease.InOutBack));
            seq.SetEase(Ease.Linear);
        }
    } 
}

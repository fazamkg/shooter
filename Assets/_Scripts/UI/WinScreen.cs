using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Faza
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform _title;
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
            var nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextIndex);
        }

        public void Appear()
        {
            _group.blocksRaycasts = true;
            _title.localScale = Vector3.zero;
            _nextButton.transform.localScale = Vector3.zero;

            var seq = DOTween.Sequence();

            seq.AppendInterval(3f);
            seq.Append(_group.DOFade(1f, 0.5f).SetEase(Ease.InOutCirc));
            seq.Append(_title.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
            seq.AppendInterval(0.3f);
            seq.Append(_nextButton.transform.DOScale(1f, 0.3f).SetEase(Ease.InOutBack));
            seq.SetEase(Ease.Linear);
        }
    } 
}

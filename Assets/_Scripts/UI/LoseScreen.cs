using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Faza
{
    public class LoseScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform _title;
        [SerializeField] private MyButton _restartButton;
        [SerializeField] private CanvasGroup _group;

        private void Awake()
        {
            _group.alpha = 0f;
            _group.blocksRaycasts = false;

            _restartButton.OnUp += RestartButton_OnUp;
        }

        private void RestartButton_OnUp()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Appear()
        {
            _group.blocksRaycasts = true;
            _title.localScale = Vector3.zero;
            _restartButton.transform.localScale = Vector3.zero;

            var seq = DOTween.Sequence();

            seq.Append(_group.DOFade(1f, 0.5f).SetEase(Ease.InOutCirc));
            seq.Append(_title.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
            seq.AppendInterval(0.3f);
            seq.Append(_restartButton.transform.DOScale(1f, 0.3f).SetEase(Ease.InOutBack));
            seq.SetEase(Ease.Linear);
        }
    } 
}

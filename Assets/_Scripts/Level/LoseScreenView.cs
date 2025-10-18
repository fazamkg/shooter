using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Faza
{
    public class LoseScreenView : MonoBehaviour
    {
        private const float FADE_DURATION = 0.5f;
        private const Ease FADE_EASE = Ease.InOutCirc;
        private const float TITLE_SCALE_DURATION = 0.3f;
        private const Ease TITLE_SCALE_EASE = Ease.OutBack;
        private const float WAIT = 0.3f;
        private const float RESTART_BUTTON_SCALE_DURATION = 0.3f;
        private const Ease RESTART_BUTTON_SCALE_EASE = Ease.InOutBack;

        [SerializeField] private RectTransform _title;
        [SerializeField] private FazaButtonView _restartButton;
        [SerializeField] private CanvasGroup _group;

        private void Awake()
        {
            _group.alpha = 0f;
            _group.blocksRaycasts = false;

            _restartButton.OnUp += RestartButton_OnUp;
        }

        public void Appear()
        {
            _group.blocksRaycasts = true;
            _title.localScale = Vector3.zero;
            _restartButton.transform.localScale = Vector3.zero;

            var seq = DOTween.Sequence();

            seq.Append(_group.DOFade(1f, FADE_DURATION).SetEase(FADE_EASE));
            seq.Append(_title.DOScale(1f, TITLE_SCALE_DURATION).SetEase(TITLE_SCALE_EASE));
            seq.AppendInterval(WAIT);
            seq.Append(_restartButton.transform.DOScale(1f, RESTART_BUTTON_SCALE_DURATION)
                .SetEase(RESTART_BUTTON_SCALE_EASE));
            seq.SetEase(Ease.Linear);
        }

        private void RestartButton_OnUp()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    } 
}

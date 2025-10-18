using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Faza
{
    public class WinScreenView : MonoBehaviour
    {
        private const float WIN_SOUND_VOLUME = 0.9f;
        private const float FADE_DURATION = 0.5f;
        private const Ease FADE_TWEEN = Ease.InOutCirc;
        private const float TITLE_SCALE_DURATION = 0.2f;
        private const Ease TITLE_SCALE_EASE = Ease.OutBack;
        private const float WAIT1 = 0.1f;
        private const float TIMER_SCALE_DURATION = 0.2f;
        private const Ease TIMER_SCALE_EASE = Ease.OutBack;
        private const float WAIT2 = 0.1f;
        private const float TIMER2_SCALE_DURATION = 0.2f;
        private const Ease TIMER2_SCALE_EASE = Ease.OutBack;
        private const float WAIT3 = 0.01f;
        private const float LEADERBOARD_PLATE_DURATION = 0.2f;
        private const Ease LEADERBOARD_PLATE_EASE = Ease.OutBack;
        private const float LEADERBOARD_WAIT_INTERVAL = 0.01f;
        private const float LEADERBOARD_LAST_WAIT_INTERVAL = 0.15f;
        private const float NEXT_BUTTON_DURATION = 0.2f;
        private const Ease NEXT_BUTTON_EASE = Ease.InOutBack;

        [SerializeField] private RectTransform _title;
        [SerializeField] private RectTransform _timerBefore;
        [SerializeField] private RectTransform _timer;
        [SerializeField] private FazaButtonView _nextButton;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private LeaderboardEntryView _firstPlace;
        [SerializeField] private LeaderboardEntryView _secondPlace;
        [SerializeField] private LeaderboardEntryView _thirdPlace;
        [SerializeField] private LeaderboardEntryView _playerPlace;

        private void Awake()
        {
            _group.alpha = 0f;
            _group.blocksRaycasts = false;

            _nextButton.OnUp += NextButton_OnUp;
        }

        public void Appear(LevelData levelData, float speed = 1f)
        {
            InitLeaderboard(levelData);

            _firstPlace.transform.localScale = Vector3.zero;
            _secondPlace.transform.localScale = Vector3.zero;
            _thirdPlace.transform.localScale = Vector3.zero;
            _playerPlace.transform.localScale = Vector3.zero;

            _group.blocksRaycasts = true;
            _title.localScale = Vector3.zero;
            _nextButton.transform.localScale = Vector3.zero;
            _timerBefore.localScale = Vector3.zero;
            _timer.localScale = Vector3.zero;

            var seq = DOTween.Sequence();

            seq.Append(_group.DOFade(1f, FADE_DURATION / speed).SetEase(FADE_TWEEN));
            seq.AppendCallback(() => FazaAudio.Play(AudioKey.WIN, WIN_SOUND_VOLUME));
            seq.Append(_title.DOScale(1f, TITLE_SCALE_DURATION / speed).SetEase(TITLE_SCALE_EASE));
            seq.AppendInterval(WAIT1 / speed);
            seq.Append(_timerBefore.DOScale(1f, TIMER_SCALE_DURATION / speed).SetEase(TIMER_SCALE_EASE));
            seq.AppendInterval(WAIT2 / speed);
            seq.Append(_timer.DOScale(1f, TIMER2_SCALE_DURATION / speed).SetEase(TIMER2_SCALE_EASE));
            seq.AppendInterval(WAIT3 / speed);

            if (_firstPlace.gameObject.activeSelf)
            {
                seq.Append(_firstPlace.transform.DOScale(1f, LEADERBOARD_PLATE_DURATION / speed)
                    .SetEase(LEADERBOARD_PLATE_EASE));
                seq.AppendInterval(LEADERBOARD_WAIT_INTERVAL / speed);
            }

            if (_secondPlace.gameObject.activeSelf)
            {
                seq.Append(_secondPlace.transform.DOScale(1f, LEADERBOARD_PLATE_DURATION / speed)
                    .SetEase(LEADERBOARD_PLATE_EASE));
                seq.AppendInterval(LEADERBOARD_WAIT_INTERVAL / speed);
            }

            if (_thirdPlace.gameObject.activeSelf)
            {
                seq.Append(_thirdPlace.transform.DOScale(1f, LEADERBOARD_PLATE_DURATION / speed)
                    .SetEase(LEADERBOARD_PLATE_EASE));
                seq.AppendInterval(LEADERBOARD_WAIT_INTERVAL / speed);
            }

            if (_playerPlace.gameObject.activeSelf)
            {
                seq.Append(_playerPlace.transform.DOScale(1f, LEADERBOARD_PLATE_DURATION / speed)
                    .SetEase(LEADERBOARD_PLATE_EASE));
                seq.AppendInterval(LEADERBOARD_LAST_WAIT_INTERVAL / speed);
            }

            seq.Append(_nextButton.transform.DOScale(1f, NEXT_BUTTON_DURATION / speed).SetEase(NEXT_BUTTON_EASE));
            seq.SetEase(Ease.Linear);
        }

        private void NextButton_OnUp()
        {
            LevelManager.Instance.LoadLevelFromSave(true);
        }

        private void InitLeaderboard(LevelData levelData)
        {
            var first = levelData.FirstPlace;
            var second = levelData.SecondPlace;
            var third = levelData.ThirdPlace;
            var player = levelData.PlayerPlace;

            if (first != null)
            {
                _firstPlace.Init(first);
            }
            else
            {
                _firstPlace.gameObject.SetActive(false);
            }

            if (second != null)
            {
                _secondPlace.Init(second);
            }
            else
            {
                _secondPlace.gameObject.SetActive(false);
            }

            if (third != null)
            {
                _thirdPlace.Init(third);
            }
            else
            {
                _thirdPlace.gameObject.SetActive(false);
            }

            if (player != null && levelData.Leaderboard.Count > 3)
            {
                _playerPlace.Init(player);
            }
            else
            {
                _playerPlace.gameObject.SetActive(false);
            }
        }
    } 
}

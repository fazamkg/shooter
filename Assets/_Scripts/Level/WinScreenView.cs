using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Faza
{
    public class WinScreenView : MonoBehaviour
    {
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

            seq.Append(_group.DOFade(1f, 0.5f / speed).SetEase(Ease.InOutCirc));
            seq.AppendCallback(() => FazaAudio.Play(AudioKey.WIN, 0.9f));
            seq.Append(_title.DOScale(1f, 0.2f / speed).SetEase(Ease.OutBack));
            seq.AppendInterval(0.1f / speed);
            seq.Append(_timerBefore.DOScale(1f, 0.2f / speed).SetEase(Ease.OutBack));
            seq.AppendInterval(0.1f / speed);
            seq.Append(_timer.DOScale(1f, 0.2f / speed).SetEase(Ease.OutBack));
            seq.AppendInterval(0.01f / speed);

            if (_firstPlace.gameObject.activeSelf)
            {
                seq.Append(_firstPlace.transform.DOScale(1f, 0.2f / speed).SetEase(Ease.OutBack));
                seq.AppendInterval(0.01f / speed);
            }

            if (_secondPlace.gameObject.activeSelf)
            {
                seq.Append(_secondPlace.transform.DOScale(1f, 0.2f / speed).SetEase(Ease.OutBack));
                seq.AppendInterval(0.01f / speed);
            }

            if (_thirdPlace.gameObject.activeSelf)
            {
                seq.Append(_thirdPlace.transform.DOScale(1f, 0.2f / speed).SetEase(Ease.OutBack));
                seq.AppendInterval(0.01f / speed);
            }

            if (_playerPlace.gameObject.activeSelf)
            {
                seq.Append(_playerPlace.transform.DOScale(1f, 0.2f / speed).SetEase(Ease.OutBack));
                seq.AppendInterval(0.15f / speed);
            }

            seq.Append(_nextButton.transform.DOScale(1f, 0.2f / speed).SetEase(Ease.InOutBack));
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

using UnityEngine;
using DG.Tweening;
using System.Linq;

namespace Faza
{
    public class UserInterfaceView : MonoBehaviour
    {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private Health _playerHealth;
        [SerializeField] private MyButtonView _settingsButton;
        [SerializeField] private SettingsView _settingsPanel;
        [SerializeField] private RectTransform _leftJoystick;
        [SerializeField] private RectTransform _rightJoystick;
        [SerializeField] private LoseScreenView _loseScreen;
        [SerializeField] private WinScreenView _winScreen;
        [SerializeField] private UpgradeScreenView _upgradeScreen;
        [SerializeField] private LevelTimer _levelTimer;
        [SerializeField] private BoosterChoiceView _boosterChoicePanel;
        [SerializeField] private AllBoostersView _allBoostersView;
        [SerializeField] private BoosterData[] _allBoosters;
        [SerializeField] private CanvasGroup _gameCanvas;
        [SerializeField] private Tutorial _tutorial;

        private bool _win;

        private void Awake()
        {
            AudioListener.volume = Settings.AudioEnabled ? 1f : 0f;

            _playerHealth.OnDeath += PlayerHealth_OnDeath;
            Health.OnDeathGlobal += Health_OnDeathGlobal;

            Settings.OnAudioEnabledChanged += Settings_OnAudioChanged;

            _settingsButton.OnUp += SettingsButton_OnUp;

            BoosterView.OnChoiceWindow += BoosterView_OnChoiceWindow;

            /*
            var isLefty = Settings.IsLefty;
            if (isLefty)
            {
                _leftJoystick.localScale = Vector3.one;
                _rightJoystick.localScale = Vector3.zero;
            }
            else
            {
                _leftJoystick.localScale = Vector3.zero;
                _rightJoystick.localScale = Vector3.one;
            }*/

            _upgradeScreen.Init();

            if (_levelData.AvailableUpgrades != null && _levelData.AvailableUpgrades.Length != 0)
            {
                _upgradeScreen.Appear(_levelData.AvailableUpgrades);
                _upgradeScreen.OnClosed += UpgradeScreen_OnClosed;
            }
            else
            {
                _levelTimer.StartTimer();
            }

            _levelData.UnlockBoosters();
            _allBoostersView.Init(_allBoosters.Where(x => x.IsUnlocked).ToList());
        }

        private void Start()
        {
            var number = _levelData.name.GetNumberPart();

            if (number == "1")
            {
                _tutorial.StartTutorial1();
            }
            else if (number == "3")
            {
                _tutorial.StartTutorial2();
            }
            else if (number == "4")
            {
                _tutorial.StartTutorial3();
            }
            else if (number == "6")
            {
                _tutorial.StartTutorial4();
            }
            else if (number == "8")
            {
                _tutorial.StartTutorial5();
            }
        }

        private void OnDestroy()
        {
            Health.OnDeathGlobal -= Health_OnDeathGlobal;
            BoosterView.OnChoiceWindow -= BoosterView_OnChoiceWindow;
        }

        public void Win(float speed = 1f)
        {
            _gameCanvas.DOFade(0f, 0.3f);

            _win = true;
            _loseScreen.gameObject.SetActive(false);

            _levelTimer.StopTimer();
            LevelManager.Instance.OnWinLevel(_levelData);

            _levelData.LoadLeaderboard(() =>
            {
                _levelData.SetCompletedTimespan(_levelTimer.Elapsed);
                _levelData.LoadLeaderboard(() =>
                {
                    _winScreen.Appear(_levelData, speed);
                },
                () =>
                {
                    _winScreen.Appear(_levelData, speed);
                });
            },
            () =>
            {
                _winScreen.Appear(_levelData, speed);
            });
        }

        private void BoosterView_OnChoiceWindow(BoosterData boosterData)
        {
            _boosterChoicePanel.Appear(boosterData);
        }

        private void UpgradeScreen_OnClosed()
        {
            _levelTimer.StartTimer();
        }

        private void Health_OnDeathGlobal()
        {
            if (EnemyInput.IsEveryoneDead)
            {
                Win();
            }
        }

        private void PlayerHealth_OnDeath()
        {
            if (_win) return;

            _gameCanvas.DOFade(0f, 0.3f);

            _levelTimer.StopTimer();

            _loseScreen.Appear();
        }

        private void Settings_OnAudioChanged()
        {
            var audioEnabled = Settings.AudioEnabled;

            if (audioEnabled)
            {
                AudioListener.volume = 1f;
            }
            else
            {
                AudioListener.volume = 0f;
            }
        }

        private void SettingsButton_OnUp()
        {
            _settingsPanel.Appear();
        }
    } 
}

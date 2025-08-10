using UnityEngine;
using DG.Tweening;
using System.Linq;

namespace Faza
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private Health _playerHealth;
        [SerializeField] private MyButton _settingsButton;
        [SerializeField] private SettingsPanel _settingsPanel;
        [SerializeField] private RectTransform _leftJoystick;
        [SerializeField] private RectTransform _rightJoystick;
        [SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private WinScreen _winScreen;
        [SerializeField] private UpgradeScreen _upgradeScreen;
        [SerializeField] private LevelTimer _levelTimer;
        [SerializeField] private BoosterChoicePanel _boosterChoicePanel;
        [SerializeField] private AllBoostersView _allBoostersView;
        [SerializeField] private BoosterData[] _allBoosters;

        private void Awake()
        {
            _playerHealth.OnDeath += PlayerHealth_OnDeath;
            Health.OnDeathGlobal += Health_OnDeathGlobal;

            Settings.OnIsLeftyChanged += Settings_OnIsLeftyChanged;

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

        private void BoosterView_OnChoiceWindow(BoosterData boosterData)
        {
            _boosterChoicePanel.Appear(boosterData);
        }

        private void UpgradeScreen_OnClosed()
        {
            _levelTimer.StartTimer();
        }

        private void OnDestroy()
        {
            Health.OnDeathGlobal -= Health_OnDeathGlobal;
            BoosterView.OnChoiceWindow -= BoosterView_OnChoiceWindow;
        }

        private void Health_OnDeathGlobal()
        {
            if (EnemyInput.IsEveryoneDead)
            {
                Win();
            }
        }

        public void Win(float speed = 1f)
        {
            _levelTimer.StopTimer();

            LevelManager.Instance.OnWinLevel(_levelData);

            _winScreen.Appear(speed);
        }

        private void PlayerHealth_OnDeath()
        {
            _levelTimer.StopTimer();

            _loseScreen.Appear();
        }

        private void Settings_OnIsLeftyChanged()
        {
            var isLefty = Settings.IsLefty;

            if (isLefty)
            {
                _leftJoystick.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
                _rightJoystick.DOScale(0f, 0.3f).SetEase(Ease.InBack);
            }
            else
            {
                _leftJoystick.DOScale(0f, 0.3f).SetEase(Ease.InBack);
                _rightJoystick.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            }
        }

        private void SettingsButton_OnUp()
        {
            _settingsPanel.Appear();
        }
    } 
}

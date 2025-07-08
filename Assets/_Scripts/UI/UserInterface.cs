using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private MyButton _settingsButton;
        [SerializeField] private SettingsPanel _settingsPanel;
        [SerializeField] private RectTransform _leftJoystick;
        [SerializeField] private RectTransform _rightJoystick;

        private void Awake()
        {
            Settings.OnIsLeftyChanged += Settings_OnIsLeftyChanged;

            _settingsButton.OnUp += SettingsButton_OnUp;

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
            }
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

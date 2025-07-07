using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private MyButton _settingsButton;
        [SerializeField] private SettingsPanel _settingsPanel;

        private void Awake()
        {
            _settingsButton.OnUp += SettingsButton_OnUp;
        }

        private void SettingsButton_OnUp()
        {
            _settingsPanel.Appear();
        }
    } 
}

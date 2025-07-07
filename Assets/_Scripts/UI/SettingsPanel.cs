using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private MyToggle _leftyToggle;
        [SerializeField] private MyButton _closeButton;

        private void Awake()
        {
            _leftyToggle.Init(Settings.IsLefty);
            _leftyToggle.OnToggle += LeftyToggle_OnToggle;

            _closeButton.OnUp += CloseButton_OnUp;

            transform.localScale = Vector3.zero;
        }

        private void CloseButton_OnUp()
        {
            Disappear();
        }

        public void Appear()
        {
            transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }

        public void Disappear()
        {
            transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
        }

        private void LeftyToggle_OnToggle()
        {
            Settings.IsLefty = _leftyToggle.IsOn;
        }
    } 
}

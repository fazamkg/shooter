using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private MyToggle _audioToggle;
        [SerializeField] private MyButton _closeButton;
        [SerializeField] private MyButton _backToMenuButton;

        private void Awake()
        {
            _audioToggle.Init(Settings.AudioEnabled);
            _audioToggle.OnToggle += AudioToggle_OnToggle;

            _backToMenuButton.OnUp += BackToMenuButton_OnUp;

            _closeButton.OnUp += CloseButton_OnUp;

            transform.localScale = Vector3.zero;
        }

        private void Start()
        {
            _backToMenuButton.gameObject.SetActive(LevelManager.Instance.MenuUnlocked);
        }

        public void Appear()
        {
            transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }

        public void Disappear()
        {
            transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
        }

        private void BackToMenuButton_OnUp()
        {
            LevelManager.Instance.LoadLevelFromSave(false);
        }

        private void CloseButton_OnUp()
        {
            Disappear();
        }

        private void AudioToggle_OnToggle()
        {
            Settings.AudioEnabled = _audioToggle.IsOn;
        }
    } 
}

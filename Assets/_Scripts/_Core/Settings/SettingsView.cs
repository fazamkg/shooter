using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class SettingsView : MonoBehaviour
    {
        private const Ease APPEAR_EASE = Ease.OutBack;
        private const Ease DISAPPEAR_EASE = Ease.InBack;
        private const float TWEEN_DURATION = 0.3f;

        [SerializeField] private FazaToggleView _audioToggle;
        [SerializeField] private FazaButtonView _closeButton;
        [SerializeField] private FazaButtonView _backToMenuButton;

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
            transform.DOScale(1f, TWEEN_DURATION).SetEase(APPEAR_EASE);
        }

        public void Disappear()
        {
            transform.DOScale(0f, TWEEN_DURATION).SetEase(DISAPPEAR_EASE);
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

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

namespace Faza
{
    public class BoosterView : MonoBehaviour
    {
        public static event Action<BoosterData> OnChoiceWindow;

        [SerializeField] private MyButton _button;
        [SerializeField] private TMP_Text[] _amountTexts;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _clock;

        private BoosterData _data;

        public BoosterData Data => _data;

        public void Init(BoosterData data)
        {
            _data = data;
            _clock.fillAmount = 0f;

            UpdateView();

            _button.OnUp += Button_OnUp;
            _data.OnUpdated += Data_OnUpdated;
            _data.OnApplied += Data_OnApplied;
        }

        private void OnDestroy()
        {
            _button.OnUp -= Button_OnUp;
            _data.OnUpdated -= Data_OnUpdated;
            _data.OnApplied -= Data_OnApplied;
        }

        private void Data_OnApplied()
        {
            _clock.fillAmount = 1f;
            _clock.DOFillAmount(0f, _data.Duration).SetEase(Ease.Linear);
        }

        private void UpdateView()
        {
            var amount = _data.AmountPref.ToString();
            foreach (var text in _amountTexts)
            {
                text.text = amount;
            }

            _icon.sprite = _data.Icon;
        }

        private void Data_OnUpdated()
        {
            UpdateView();
        }

        private void Button_OnUp()
        {
            _data.OnTap(OpenWindow);
        }

        private void OpenWindow()
        {
            OnChoiceWindow?.Invoke(_data);
        }
    } 
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Faza
{
    public class BoosterView : MonoBehaviour
    {
        public static event Action<BoosterData> OnChoiceWindow;

        [SerializeField] private MyButton _button;
        [SerializeField] private TMP_Text[] _amountTexts;
        [SerializeField] private Image _icon;

        private BoosterData _data;

        public void Init(BoosterData data)
        {
            _data = data;

            UpdateView();

            _button.OnUp += Button_OnUp;

            _data.OnUpdated += Data_OnUpdated;
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

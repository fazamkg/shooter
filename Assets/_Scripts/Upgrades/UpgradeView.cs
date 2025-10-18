using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Globalization;

namespace Faza
{
    public class UpgradeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _title;
        [SerializeField] private TMP_Text[] _cost;
        [SerializeField] private TMP_Text[] _description;
        [SerializeField] private Image _image;
        [SerializeField] private MyButtonView _button;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private Sprite _notEnoughSprite;

        private UpgradeGroupData _group;

        public void Init(UpgradeGroupData group)
        {
            _group = group;

            _button.OnUp += OnClick;

            Currency.OnCoinsAdded += Currency_OnCoinsAdded;
            Currency.OnCoinsRemoved += Currency_OnCoinsRemoved;

            UpdateView();
        }

        private void OnDisable()
        {
            Currency.OnCoinsAdded -= Currency_OnCoinsAdded;
            Currency.OnCoinsRemoved -= Currency_OnCoinsRemoved;
        }

        private void Currency_OnCoinsRemoved(float oldValue, float newValue)
        {
            UpdateView();
        }

        private void Currency_OnCoinsAdded(float oldValue, float newValue, Vector3 worldPosition)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var data = _group.GetCurrentToPurhase();

            var title = data.Name;
            var cost = data.IsMaxed ? "МАКС" : data.Cost.ToString(CultureInfo.InvariantCulture);
            var desc = data.Description;

            foreach (var thing in _title)
            {
                thing.text = title;
            }

            foreach (var thing in _description)
            {
                thing.text = desc;
            }

            foreach (var thing in _cost)
            {
                thing.text = cost;
            }

            _image.sprite = data.Icon;

            var cantBuy = data.IsMaxed || data.Cost > Currency.Coins;
            _buttonImage.overrideSprite = cantBuy ? _notEnoughSprite : null;
        }

        private void OnClick()
        {
            var purchased = _group.Purchase();
            if (purchased)
            {
                UpdateView();
            }
        }
    } 
}

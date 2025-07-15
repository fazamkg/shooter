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
        [SerializeField] private MyButton _button;

        private UpgradeGroupData _group;

        public void Init(UpgradeGroupData group)
        {
            _group = group;

            _button.OnUp += OnClick;

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

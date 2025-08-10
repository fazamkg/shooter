using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Faza
{
    public class BoosterChoicePanel : MonoBehaviour
    {
        [SerializeField] private MyButton _closeButton;
        [SerializeField] private MyButton _mainButton;
        [SerializeField] private MyButton _altButton;
        [SerializeField] private Image _mainIcon;
        [SerializeField] private Image _altIcon;
        [SerializeField] private Image _boosterIcon;
        [SerializeField] private TMP_Text[] _titleTexts;
        [SerializeField] private TMP_Text[] _mainCostTexts;
        [SerializeField] private TMP_Text[] _altCostTexts;
        [SerializeField] private TMP_Text[] _mainCounterTexts;
        [SerializeField] private TMP_Text[] _altCounterTexts;
        [SerializeField] private TMP_Text[] _mainRewardAmountTexts;
        [SerializeField] private TMP_Text[] _altRewardAmountTexts;
        [SerializeField] private TMP_Text[] _descTexts;
        [SerializeField] private TMP_Text[] _boosterDescTexts;

        private BoosterData _booster;

        private void Awake()
        {
            _closeButton.OnUp += CloseButton_OnUp;
            _mainButton.OnUp += MainButton_OnUp;
            _altButton.OnUp += AltButton_OnUp;

            transform.localScale = Vector3.zero;
        }

        private void AltButton_OnUp()
        {
            _booster.PurchaseAlt();

            UpdateView();
        }

        private void MainButton_OnUp()
        {
            _booster.Purchase();

            UpdateView();
        }

        private void CloseButton_OnUp()
        {
            Disappear();
        }

        private void UpdateView()
        {
            _boosterIcon.sprite = _booster.Icon;

            var title = _booster.Title;
            foreach (var text in _titleTexts)
            {
                text.text = title;
            }

            var desc = _booster.WindowDescription;
            foreach (var text in _descTexts)
            {
                text.text = desc;
            }

            var boosterDesc = _booster.ItemDescription;
            foreach (var text in _boosterDescTexts)
            {
                text.text = boosterDesc;
            }

            var mainSpend = _booster.MainSpendAction;
            var altSpend = _booster.AltSpendAction;

            var mainCost = mainSpend.Cost;
            var altCost = altSpend.Cost;

            foreach (var text in _mainCostTexts)
            {
                text.text = mainCost;
            }

            foreach (var text in _altCostTexts)
            {
                text.text = altCost;
            }

            _mainIcon.sprite = mainSpend.Sprite;
            _altIcon.sprite = altSpend.Sprite;

            var mainCounter = $"{_booster.PurchaseCountPref}/{_booster.MainPurchaseCount}";
            var altCounter = $"{_booster.AltPurchaseCountPref}/{_booster.AltPurchaseCount}";
            foreach (var text in _mainCounterTexts)
            {
                text.text = mainCounter;
            }
            foreach (var text in _altCounterTexts)
            {
                text.text = altCounter;
            }

            var mainReward = $"x{_booster.BoosterAmount}";
            var altReward = $"x{_booster.AltBoosterAmount}";
            foreach (var text in _mainRewardAmountTexts)
            {
                text.text = mainReward;
            }
            foreach (var text in _altRewardAmountTexts)
            {
                text.text = altReward;
            }
        }

        public void Appear(BoosterData booster)
        {
            _booster = booster;
            transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);

            UpdateView();
        }

        public void Disappear()
        {
            transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
        }
    } 
}

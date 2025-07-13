using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Faza
{
    public class CoinsView : MonoBehaviour
    {
        [SerializeField] private RectTransform _icon;
        [SerializeField] private TMP_Text[] _texts;
        [SerializeField] private RectTransform _coinPrefab;

        private void Awake()
        {
            Currency.OnCoinsAdded += Currency_OnCoinsAdded;

            foreach (var text in _texts)
            {
                text.text = Currency.Coins.ToString();
            }
        }

        private void OnDestroy()
        {
            Currency.OnCoinsAdded -= Currency_OnCoinsAdded;
        }

        private void Currency_OnCoinsAdded(float oldValue, float newValue, Vector3 worldPosition)
        {
            var coin = Instantiate(_coinPrefab, transform);

            coin.position = Camera.main.WorldToScreenPoint(worldPosition);

            var seq = DOTween.Sequence();
            seq.Append(coin.DOJumpAnchorPos(_icon.anchoredPosition, 20f, 1, 0.5f).SetEase(Ease.InCirc));
            seq.AppendCallback(() => coin.gameObject.SetActive(false));
            seq.Append(_icon.DOScale(1.2f, 0.15f).SetEase(Ease.Linear));
            seq.AppendCallback(() =>
            {
                foreach (var text in _texts)
                {
                    text.text = newValue.ToString();
                }
            });
            seq.Append(_icon.DOScale(1f, 0.15f).SetEase(Ease.Linear));
        }
    } 
}

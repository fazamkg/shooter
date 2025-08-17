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
        [SerializeField] private bool _instantUpdate;

        private void Awake()
        {
            Currency.OnCoinsAdded += Currency_OnCoinsAdded;
            Currency.OnCoinsRemoved += Currency_OnCoinsRemoved;

            foreach (var text in _texts)
            {
                text.text = Currency.Coins.ToString();
            }
        }
        
        private void OnDestroy()
        {
            Currency.OnCoinsAdded -= Currency_OnCoinsAdded;
            Currency.OnCoinsRemoved -= Currency_OnCoinsRemoved;
        }

        private void Currency_OnCoinsRemoved(float oldValue, float newValue)
        {
            foreach (var text in _texts)
            {
                text.text = newValue.ToString();
            }
        }


        private void Currency_OnCoinsAdded(float oldValue, float newValue, Vector3 worldPosition)
        {
            if (_instantUpdate)
            {
                foreach (var text in _texts)
                {
                    text.text = Currency.Coins.ToString();
                }
                return;
            }

            var coin = Instantiate(_coinPrefab, transform);

            coin.position = Camera.main.WorldToScreenPoint(worldPosition);

            var seq = DOTween.Sequence();
            seq.Append(coin.DOJumpAnchorPos(_icon.anchoredPosition, 20f, 1, 0.5f).SetEase(Ease.InCirc));
            seq.AppendCallback(() => coin.gameObject.SetActive(false));
            seq.Append(_icon.DOScale(1.2f, 0.15f).SetEase(Ease.Linear));
            seq.AppendCallback(() =>
            {
                MyAudio.Play("coin", 0.5f);

                foreach (var text in _texts)
                {
                    text.text = newValue.ToString();
                }
            });
            seq.Append(_icon.DOScale(1f, 0.15f).SetEase(Ease.Linear));
        }
    } 
}

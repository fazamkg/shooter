using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Faza
{
    public class CoinsView : MonoBehaviour
    {
        private const float JUMP_POWER = 20f;
        private const float JUMP_DURATION = 0.5f;
        private const Ease JUMP_EASE = Ease.InCirc;
        private const float ICON_TARGET_SCALE = 1.2f;
        private const float ICON_TWEEN_DURATION = 0.15f;
        private const Ease ICON_EASE = Ease.Linear;
        private const float COIN_SOUND_VOLUME = 0.5f;

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
            seq.Append(coin.DOJumpAnchorPos(_icon.anchoredPosition, JUMP_POWER, 1, JUMP_DURATION).SetEase(JUMP_EASE));
            seq.AppendCallback(() => coin.gameObject.SetActive(false));
            seq.Append(_icon.DOScale(ICON_TARGET_SCALE, ICON_TWEEN_DURATION).SetEase(ICON_EASE));
            seq.AppendCallback(() =>
            {
                FazaAudio.Play(AudioKey.COIN, COIN_SOUND_VOLUME);

                foreach (var text in _texts)
                {
                    text.text = newValue.ToString();
                }
            });
            seq.Append(_icon.DOScale(1f, ICON_TWEEN_DURATION).SetEase(ICON_EASE));
        }
    } 
}

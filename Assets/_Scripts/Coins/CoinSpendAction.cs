using System;
using UnityEngine;

namespace Faza
{
    [Serializable]
    public class CoinSpendAction : SpendAction
    {
        private const string COIN_SPRITE_PATH = "Coin";

        [SerializeField] private float _cost;

        public override Sprite Sprite => Resources.Load<Sprite>(COIN_SPRITE_PATH);

        public override string Cost => _cost == 0f ? Localization.Get(LocalizationKey.FREE) : _cost.ToString();

        public override bool CanSpend()
        {
            return Currency.Coins >= _cost;
        }

        public override void Spend(Action onSucess, Action onFailure)
        {
            if (CanSpend())
            {
                Currency.RemoveCoins(_cost);
                onSucess?.Invoke();
            }
            else
            {
                onFailure?.Invoke();
            }
        }
    }
}

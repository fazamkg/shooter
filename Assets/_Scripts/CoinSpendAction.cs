using System;
using UnityEngine;

namespace Faza
{
    [Serializable]
    public class CoinSpendAction : SpendAction
    {
        [SerializeField] private float _cost;

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

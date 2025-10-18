using UnityEngine;

namespace Faza
{
    public static class Currency
    {
        public delegate void DeltaEventWithPos(float oldValue, float newValue, Vector3 worldPosition);
        public delegate void DeltaEvent(float oldValue, float newValue);

        public static event DeltaEventWithPos OnCoinsAdded;
        public static event DeltaEvent OnCoinsRemoved;

        public static float Coins
        {
            get => Storage.GetFloat(StorageKey.COINS);
            set => Storage.SetFloat(StorageKey.COINS, value);
        }

        public static void AddCoins(float toAdd, Vector3 worldPosition)
        {
            var old = Coins;
            Coins += toAdd;

            OnCoinsAdded?.Invoke(old, Coins, worldPosition);
        }

        public static void RemoveCoins(float toRemove)
        {
            var old = Coins;
            Coins -= toRemove;
            Coins = Mathf.Max(Coins, 0f);

            OnCoinsRemoved?.Invoke(old, Coins);
        }
    } 
}

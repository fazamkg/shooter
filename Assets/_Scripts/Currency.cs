using UnityEngine;

namespace Faza
{
    public static class Currency
    {
        public delegate void DeltaEvent(float oldValue, float newValue, Vector3 worldPosition);

        public static event DeltaEvent OnCoinsAdded;

        public static float Coins
        {
            get => Storage.GetFloat("faza_coins");
            set => Storage.SetFloat("faza_coins", value);
        }

        public static void AddCoins(float toAdd, Vector3 worldPosition)
        {
            var old = Coins;
            Coins += toAdd;

            OnCoinsAdded?.Invoke(old, Coins, worldPosition);
        }
    } 
}

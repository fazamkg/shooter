namespace Faza
{
    public static class Currency
    {
        public delegate void DeltaEvent(float oldValue, float newValue);

        public static event DeltaEvent OnCoinsAdded;

        public static float Coins
        {
            get => Storage.GetFloat("faza_coins");
            set
            {
                var oldValue = Coins;

                Storage.SetFloat("faza_coins", value);

                OnCoinsAdded?.Invoke(oldValue, value);
            }
        }
    } 
}

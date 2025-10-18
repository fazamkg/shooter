namespace Faza
{
    public static class StorageKey
    {
        public const string AUDIO_ENABLED = "faza_audio";
        public const string COINS = "faza_coins";
        public const string LEVEL_INDEX = "faza_level";
        public const string TUTORIAL1 = "faza_tutorial_1";
        public const string TUTORIAL2 = "faza_tutorial_2";
        public const string TUTORIAL3 = "faza_tutorial_3";
        public const string TUTORIAL4 = "faza_tutorial_4";
        public const string TUTORIAL5 = "faza_tutorial_5";

        public static string GetBoosterAmountKey(string id)
        {
            return $"{id}_amount";
        }

        public static string GetBoosterPurchaseCountKey(string id)
        {
            return $"{id}_purchases";
        }

        public static string GetBoosterAltPurchaseCountKey(string id)
        {
            return $"{id}_alt_purchases";
        }

        public static string GetBoosterUnlockedKey(string id)
        {
            return $"{id}_unlocked";
        }

        public static string GetLeaderboardTimeKey(string name)
        {
            return $"faza_{name}_timespan";
        }

        public static string GetLeaderboardRankKey(string name)
        {
            return $"faza_{name}_rank";
        }
    } 
}

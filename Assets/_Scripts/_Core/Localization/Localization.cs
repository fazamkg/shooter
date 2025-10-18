using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using YG;

namespace Faza
{
    [CreateAssetMenu]
    public class Localization : ScriptableObject
    {
        private const string LOCALIZATION_PATH = "Localization";
        private const string RUSSIAN = "ru";
        private const string ENGLISH = "en";

        [SerializeField] private List<LocalizationEntry> _entries;

        private static Localization _instance;

        private static Localization Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<Localization>(LOCALIZATION_PATH);
                }

                return _instance;
            }
        }

        public static string Get(string key)
        {
            var entries = Instance._entries;

            var entry = entries.FirstOrDefault(x => x.Key == key);

            if (entry == null) return key;

            return YandexGame.lang switch
            {
                RUSSIAN => entry.Russian,
                ENGLISH => entry.English,
                _ => entry.Russian
            };
        }
    } 
}

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using YG;

namespace Faza
{
    [Serializable]
    public class LocalizationEntry
    {
        [SerializeField] private string _key;
        [SerializeField, TextArea] private string _russian;
        [SerializeField, TextArea] private string _english;

        public string Key => _key;
        public string Russian => _russian;
        public string English => _english;
    }

    [CreateAssetMenu]
    public class Localization : ScriptableObject
    {
        [SerializeField] private List<LocalizationEntry> _entries;

        private static Localization _instance;

        private static Localization Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<Localization>("Localization");
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
                "ru" => entry.Russian,
                "en" => entry.English,
                _ => entry.Russian
            };
        }
    } 
}

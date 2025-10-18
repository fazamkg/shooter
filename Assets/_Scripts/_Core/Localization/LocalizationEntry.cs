using UnityEngine;
using System;

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
}

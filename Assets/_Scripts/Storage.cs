using System;
using System.Globalization;
using UnityEngine;

namespace Faza
{
    public static class Storage
    {
        public static float GetFloat(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static TimeSpan GetTimeSpan(string key, TimeSpan defaultValue = default)
        {
            var loadedValue = PlayerPrefs.GetString(key, "");
            if (loadedValue == "") return defaultValue;

            var parsed = TimeSpan.TryParse(loadedValue, CultureInfo.InvariantCulture, out var result);
            if (parsed == false) return defaultValue;

            return result;
        }

        public static void SetTimeSpan(string key, TimeSpan value)
        {
            var toSave = value.ToString("c");
            PlayerPrefs.SetString(key, toSave);
        }
    } 
}

using System;
using System.Globalization;
using YG;

namespace Faza
{
    public static class Storage
    {
        public static float GetFloat(string key, float defaultValue = 0)
        {
            if (YandexGame.savesData.floats.ContainsKey(key) == false) return defaultValue;

            return YandexGame.savesData.floats[key];
        }

        public static void SetFloat(string key, float value)
        {
            YandexGame.savesData.floats[key] = value;
            YandexGame.SaveProgress();
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            if (YandexGame.savesData.ints.ContainsKey(key) == false) return defaultValue;

            return YandexGame.savesData.ints[key];
        }

        public static void SetInt(string key, int value)
        {
            YandexGame.savesData.ints[key] = value;
            YandexGame.SaveProgress();
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            if (YandexGame.savesData.ints.ContainsKey(key) == false) return defaultValue;

            return YandexGame.savesData.ints[key] == 1;
        }

        public static void SetBool(string key, bool value)
        {
            YandexGame.savesData.ints[key] = value ? 1 : 0;
            YandexGame.SaveProgress();
        }

        public static TimeSpan GetTimeSpan(string key, TimeSpan defaultValue = default)
        {
            if (YandexGame.savesData.strings.ContainsKey(key) == false) return defaultValue;

            var loadedValue = YandexGame.savesData.strings[key];
            if (loadedValue == "") return defaultValue;

            var parsed = TimeSpan.TryParse(loadedValue, CultureInfo.InvariantCulture, out var result);
            if (parsed == false) return defaultValue;

            return result;
        }

        public static void SetTimeSpan(string key, TimeSpan value)
        {
            var toSave = value.ToString("c");
            YandexGame.savesData.strings[key] = toSave;
            YandexGame.SaveProgress();
        }
    } 
}

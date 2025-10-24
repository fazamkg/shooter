using System;
using System.Globalization;
using UnityEngine;

#if YG_PLUGIN_YANDEX_GAME
using YG;
#endif

namespace Faza
{
    public static class Storage
    {
        public static float GetFloat(string key, float defaultValue = 0)
        {
#if YG_PLUGIN_YANDEX_GAME
            if (YandexGame.savesData.floats.ContainsKey(key) == false) return defaultValue;

            return YandexGame.savesData.floats[key];
#else
            return PlayerPrefs.GetFloat(key, defaultValue);
#endif
        }

        public static void SetFloat(string key, float value)
        {
#if YG_PLUGIN_YANDEX_GAME
            YandexGame.savesData.floats[key] = value;
            YandexGame.SaveProgress();
#else
            PlayerPrefs.SetFloat(key, value);
#endif
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
#if YG_PLUGIN_YANDEX_GAME
            if (YandexGame.savesData.ints.ContainsKey(key) == false) return defaultValue;

            return YandexGame.savesData.ints[key];
#else
            return PlayerPrefs.GetInt(key, defaultValue);
#endif
        }

        public static void SetInt(string key, int value)
        {
#if YG_PLUGIN_YANDEX_GAME
            YandexGame.savesData.ints[key] = value;
            YandexGame.SaveProgress();
#else
            PlayerPrefs.SetInt(key, value);
#endif
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
#if YG_PLUGIN_YANDEX_GAME
            if (YandexGame.savesData.ints.ContainsKey(key) == false) return defaultValue;

            return YandexGame.savesData.ints[key] == 1;
#else
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
#endif
        }

        public static void SetBool(string key, bool value)
        {
#if YG_PLUGIN_YANDEX_GAME
            YandexGame.savesData.ints[key] = value ? 1 : 0;
            YandexGame.SaveProgress();
#else
            PlayerPrefs.SetInt(key, value ? 1 : 0);
#endif
        }

        public static TimeSpan GetTimeSpan(string key, TimeSpan defaultValue = default)
        {
#if YG_PLUGIN_YANDEX_GAME
            if (YandexGame.savesData.strings.ContainsKey(key) == false) return defaultValue;

            var loadedValue = YandexGame.savesData.strings[key];
            if (loadedValue == "") return defaultValue;

            var parsed = TimeSpan.TryParse(loadedValue, CultureInfo.InvariantCulture, out var result);
            if (parsed == false) return defaultValue;

            return result;
#else
            var loadedValue = PlayerPrefs.GetString(key, "");
            if (loadedValue == "") return defaultValue;

            var parsed = TimeSpan.TryParse(loadedValue, CultureInfo.InvariantCulture, out var result);
            if (parsed == false) return defaultValue;

            return result;
#endif
        }

        public static void SetTimeSpan(string key, TimeSpan value)
        {
#if YG_PLUGIN_YANDEX_GAME
            var toSave = value.ToString("c");
            YandexGame.savesData.strings[key] = toSave;
            YandexGame.SaveProgress();
#else
            var toSave = value.ToString("c");
            PlayerPrefs.SetString(key, toSave);
#endif
        }
    } 
}

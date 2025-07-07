using UnityEngine;

namespace Faza
{
    public static class Storage
    {
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
    } 
}

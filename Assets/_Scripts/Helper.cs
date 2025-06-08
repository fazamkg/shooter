using System;
using System.Globalization;
using UnityEngine;

namespace Faza
{
    public static class Helper
    {
        public static void Toggle(this GameObject gameObject)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public static Vector3 WithY(this Vector3 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        public static bool IsZero(this float value)
        {
            return Mathf.Approximately(value, 0f);
        }

        public static float ToFloat(this string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture);
        }

        public static bool ToBool(this string value)
        {
            var parsedAsInt = int.TryParse(value, out var number);
            if (parsedAsInt) return number == 1;

            return bool.Parse(value);
        }

        public static int ToInt(this string value)
        {
            return int.Parse(value, CultureInfo.InvariantCulture);
        }

        public static KeyCode ToKeyCode(this string value)
        {
            return Enum.Parse<KeyCode>(value, true);
        }

        public static float Abs(this float value)
        {
            return Mathf.Abs(value);
        }
    }
}
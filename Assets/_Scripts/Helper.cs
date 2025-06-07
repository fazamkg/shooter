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
    }
}
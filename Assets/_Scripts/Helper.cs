using UnityEngine;

namespace Faza
{
    public static class Helper
    {
        public static void Toggle(this GameObject gameObject)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
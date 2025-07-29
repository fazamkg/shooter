using System;
using System.Globalization;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using UnityEngine.Animations.Rigging;

namespace Faza
{
    public static class Helper
    {
        public static T GetRandom<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        public static Tween DOFloat(this Animator animator, string property, float to, float duration)
        {
            return DOTween.To(() => animator.GetFloat(property),
                x => animator.SetFloat(property, x), to, duration);
        }

        public static void Toggle(this GameObject gameObject)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public static Vector3 WithY(this Vector3 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        public static Vector3 DeltaY(this Vector3 vector, float delta)
        {
            vector.y += delta;
            return vector;
        }

        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            vector.z = z;
            return vector;
        }

        public static Color WithA(this Color color, float a)
        {
            color.a = a;
            return color;
        }

        public static Color AddHue(this Color color, float addH)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            return Color.HSVToRGB(h + addH, s, v);
        }

        public static void SetAnchorX(this RectTransform rt, float x)
        {
            var pos = rt.anchoredPosition;
            pos.x = x;
            rt.anchoredPosition = pos;
        }

        public static void SetAnchorY(this RectTransform rt, float y)
        {
            var pos = rt.anchoredPosition;
            pos.y = y;
            rt.anchoredPosition = pos;
        }

        public static void SetWidth(this RectTransform rt, float width)
        {
            var temp = rt.sizeDelta;
            temp.x = width;
            rt.sizeDelta = temp;
        }

        public static void SetHeight(this RectTransform rt, float height)
        {
            var temp = rt.sizeDelta;
            temp.y = height;
            rt.sizeDelta = temp;
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

        public static float Sign(this float value)
        {
            return Mathf.Sign(value);
        }

        private static GameObject _lastActive;
        public static void ActivateSingle(this GameObject gameObject)
        {
            if (_lastActive != null) _lastActive.SetActive(false);
            gameObject.SetActive(true);
            _lastActive = gameObject;
        }

        public static float SqrDist(Vector3 a, Vector3 b)
        {
            return (a - b).sqrMagnitude;
        }

        public static Tween DOWeight(this Rig rig, float to, float duration)
        {
            return DOTween.To(() => rig.weight, x => rig.weight = x, to, duration);
        }
    }
}
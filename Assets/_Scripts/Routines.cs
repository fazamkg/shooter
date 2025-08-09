using UnityEngine;
using System.Collections;

namespace Faza
{
    public class Routines : MonoBehaviour
    {
        private static Routines _routines;

        private void Awake()
        {
            _routines = this;
            DontDestroyOnLoad(gameObject);
        }

        public static void StartCoroutine_(IEnumerator routine)
        {
            _routines.StartCoroutine(routine);
        }

        public static void StopCoroutine_(IEnumerator routine)
        {
            _routines.StopCoroutine(routine);
        }

        public static void StopCoroutine_(Coroutine routine)
        {
            _routines.StopCoroutine(routine);
        }
    }
}

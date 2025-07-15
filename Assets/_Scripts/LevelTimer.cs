using System.Collections;
using UnityEngine;
using System;

namespace Faza
{
    public class LevelTimer : MonoBehaviour
    {
        public event Action OnTick;

        private TimeSpan _elapsed;

        public TimeSpan Elapsed => _elapsed;

        public string AsText => $"{(int)_elapsed.TotalHours:D2}:{_elapsed.Minutes:D2}:{_elapsed.Seconds:D2}";

        public void StartTimer()
        {
            StartCoroutine(Tick());
        }

        public void StopTimer()
        {
            StopAllCoroutines();
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1f);
                _elapsed += TimeSpan.FromSeconds(1d);
                OnTick?.Invoke();
            }
        }
    } 
}

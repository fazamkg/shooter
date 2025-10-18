using System.Collections;
using UnityEngine;
using System;

namespace Faza
{
    public class LevelTimer : MonoBehaviour
    {
        public event Action OnTick;

        private const string TIME_FORMAT = @"mm\:ss\.fff";
        private const float TICK_INTERVAL_SECONDS = 0.1f;

        private TimeSpan _elapsed;
        private double _from;

        public TimeSpan Elapsed => _elapsed;

        public string AsText => _elapsed.ToString(TIME_FORMAT);

        private void Update()
        {
            var time = Time.realtimeSinceStartupAsDouble;

            var delta = time - _from;

            _elapsed = TimeSpan.FromSeconds(delta);
        }

        public void StartTimer()
        {
            _from = Time.realtimeSinceStartupAsDouble;
            StartCoroutine(TickCoroutine());
        }

        public void StopTimer()
        {
            enabled = false;
            StopAllCoroutines();
        }

        private IEnumerator TickCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(TICK_INTERVAL_SECONDS);
                OnTick?.Invoke();
            }
        }
    } 
}

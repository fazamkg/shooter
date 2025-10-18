using System.Collections;
using UnityEngine;
using System;

namespace Faza
{
    public class LevelTimer : MonoBehaviour
    {
        public event Action OnTick;

        private TimeSpan _elapsed;
        private double _from;

        public TimeSpan Elapsed => _elapsed;

        public string AsText => _elapsed.ToString(@"mm\:ss\.fff");

        private void Update()
        {
            var time = Time.realtimeSinceStartupAsDouble;

            var delta = time - _from;

            _elapsed = TimeSpan.FromSeconds(delta);
        }

        public void StartTimer()
        {
            _from = Time.realtimeSinceStartupAsDouble;
            StartCoroutine(Tick());
        }

        public void StopTimer()
        {
            enabled = false;
            StopAllCoroutines();
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                OnTick?.Invoke();
            }
        }
    } 
}

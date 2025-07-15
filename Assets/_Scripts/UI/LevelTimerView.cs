using UnityEngine;
using TMPro;

namespace Faza
{
    public class LevelTimerView : MonoBehaviour
    {
        [SerializeField] private LevelTimer _timer;
        [SerializeField] private TMP_Text[] _texts;

        private void Awake()
        {
            var asString = _timer.AsText;

            foreach (var text in _texts)
            {
                text.text = asString;
            }

            _timer.OnTick += Timer_OnTick;
        }

        private void Timer_OnTick()
        {
            var asString = _timer.AsText;

            foreach (var text in _texts)
            {
                text.text = asString;
            }
        }
    } 
}

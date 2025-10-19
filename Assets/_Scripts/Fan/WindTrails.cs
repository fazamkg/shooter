using UnityEngine;

namespace Faza
{
    public class WindTrails : MonoBehaviour
    {
        private const float MAX_TIME = 100_000f;

        [SerializeField] private Fan _fan;
        [SerializeField] private TrailRenderer[] _trails;
        [SerializeField] private TrailRenderer[] _copyTrails;
        [SerializeField] private float _timeLength;
        [SerializeField] private float _distance;
        [SerializeField] private float _amplitude;
        [SerializeField] private float _frequency;
        [SerializeField] private float _diff;

        private Vector3[] _originalPositions;
        private float[] _randomOffsets;
        private float[] _previousTimes;

        private float _time;

        private void Start()
        {
            _originalPositions = new Vector3[_trails.Length];
            _randomOffsets = new float[_trails.Length];
            _previousTimes = new float[_trails.Length];

            for (var i = 0; i < _trails.Length; i++)
            {
                _originalPositions[i] = _trails[i].transform.position;
                _randomOffsets[i] = Random.Range(0f, _timeLength);
                _previousTimes[i] = Mathf.Repeat(_time + _randomOffsets[i], _timeLength);
            }
        }

        private void Update()
        {
            if (_fan.IsOn == false) return;

            _time += Time.deltaTime;
            _time = Mathf.Repeat(_time, MAX_TIME);

            var direction = transform.forward;

            for (var i = 0; i < _trails.Length; i++)
            {
                var time = Mathf.Repeat(_time + _randomOffsets[i], _timeLength);

                var og = _originalPositions[i];
                var target = og + direction * _distance;

                var y = Mathf.Sin(time * _frequency) * _amplitude;

                var weight = time / _timeLength;
                var newPos = Vector3.Lerp(og, target, weight);
                newPos.y += y;

                if (time < _previousTimes[i])
                {
                    var trail = _trails[i];
                    trail.emitting = false;

                    _trails[i] = _copyTrails[i];
                    _copyTrails[i] = trail;


                    _trails[i].emitting = true;
                }

                if (time > _timeLength * 0.5f)
                {
                    _copyTrails[i].transform.position = _originalPositions[i];
                }

                _trails[i].transform.position = newPos;

                _previousTimes[i] = time;
            }
        }
    } 
}

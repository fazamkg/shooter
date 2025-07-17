using UnityEngine;
using Faza;

public class WindTrails : MonoBehaviour
{
    [SerializeField] private TrailRenderer[] _trails;
    [SerializeField] private float _timeLength;
    [SerializeField] private float _distance;
    [SerializeField] private float _amplitude;
    [SerializeField] private float _frequency;
    [SerializeField] private float _diff;

    private Vector3[] _originalPositions;
    private float[] _randomOffsets;
    private float[] _previousTimes;

    private void Start()
    {
        _originalPositions = new Vector3[_trails.Length];
        _randomOffsets = new float[_trails.Length];
        _previousTimes = new float[_trails.Length];

        for (var i = 0; i < _trails.Length; i++)
        {
            _originalPositions[i] = _trails[i].transform.position;
            _randomOffsets[i] = Random.Range(0f, _timeLength);
            _previousTimes[i] = Mathf.Repeat(Time.time + _randomOffsets[i], _timeLength);
        }
    }

    private void Update()
    {
        var direction = transform.forward;

        for (var i = 0; i < _trails.Length; i++)
        {
            var time = Mathf.Repeat(Time.time + _randomOffsets[i], _timeLength);

            var og = _originalPositions[i];
            var target = og + direction * _distance;

            var y = Mathf.Sin(time * _frequency) * _amplitude;

            var weight = time / _timeLength;
            var newPos = Vector3.Lerp(og, target, weight);
            newPos.y += y;

            _trails[i].transform.position = newPos;

            if (time < _previousTimes[i])
            {
                _trails[i].Clear();
            }

            _previousTimes[i] = time;
        }
    }
}

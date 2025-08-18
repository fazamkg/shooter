using UnityEngine;

namespace Faza
{
    public class Fan : MonoBehaviour
    {
        [SerializeField] private Transform _toRotate;
        [SerializeField] private float _speed;
        [SerializeField] private float _strength;
        [SerializeField] private bool _repeatToggle;
        [SerializeField] private float _timeOn;
        [SerializeField] private float _timeOff;
        [SerializeField] private bool _scaleWithDistance = true;
        [SerializeField] private AudioSource _source;

        private float _timer;
        private bool _on = true;

        public bool IsOn => _on;

        private void Update()
        {
            if (_repeatToggle)
            {
                _timer += Time.deltaTime;

                var time = _on ? _timeOn : _timeOff;

                if (_timer > time)
                {
                    _timer = 0f;
                    _on = !_on;

                    if (_on)
                    {
                        _source.Play();
                    }
                    else
                    {
                        _source.Stop();
                    }
                }
            }

            if (_on)
            {
                _toRotate.Rotate(0f, Time.deltaTime * _speed, 0f, Space.Self);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_on == false) return;

            var character = other.GetComponent<Character>();
            if (character == false) return;

            var direction = transform.up;

            var accel = _strength * Time.fixedDeltaTime * direction;
            if (_scaleWithDistance)
            {
                var distance = Vector3.Distance(transform.position, character.transform.position);
                var oppDist = 1f / distance;

                accel *= oppDist;
            }

            character.AddVelocity(accel);
        }
    } 
}

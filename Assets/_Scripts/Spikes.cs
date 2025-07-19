using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace Faza
{
    public class Spikes : MonoBehaviour
    {
        [SerializeField] private bool _isUp = true;
        [SerializeField] private float _timerShift;
        [SerializeField] private float _damage;
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _spikesHolder;
        [SerializeField] private float _hiddenTime;
        [SerializeField] private float _shownTime;
        [SerializeField] private Vector3 _up = new(0.5f, 0f, -1f);
        [SerializeField] private Vector3 _down = new(0.5f, -1f, -1f);

        private float _timer;
        private bool _shown = true;

        private List<Character> _captured = new();

        private void Awake()
        {
            if (_isUp == false)
            {
                _spikesHolder.localPosition = _down;
                _shown = false;
                _collider.enabled = true;
            }
            _timer += _timerShift;
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            var time = _shown ? _shownTime : _hiddenTime;

            if (_timer >= time)
            {
                _timer = 0f;
                _shown = !_shown;

                if (_shown)
                {
                    _spikesHolder.DOLocalMove(_up, 0.3f)
                        .SetEase(Ease.InExpo)
                        .OnComplete(() =>
                        {
                            var center = transform.position + new Vector3(1f, 1f, -1f);

                            var colliders = Physics.OverlapBox(center, Vector3.one);
                            foreach (var collider in colliders)
                            {
                                var health = collider.GetComponent<Health>();
                                if (health == false) continue;
                                health.TakeDamage(_damage, Vector3.up, false);

                                var character = health.GetComponent<Character>();
                                _captured.Add(character);
                                character.enabled = false;
                            }

                            _collider.enabled = true;
                        });
                }
                else
                {
                    _spikesHolder.DOLocalMove(_down, 0.3f)
                        .SetEase(Ease.InExpo)
                        .OnComplete(() =>
                        {
                            _collider.enabled = false;

                            foreach (var captured in _captured)
                            {
                                captured.enabled = true;
                            }
                            _captured.Clear();
                        });
                }
            }
        }
    } 
}

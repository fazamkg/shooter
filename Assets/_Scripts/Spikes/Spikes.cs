using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace Faza
{
    public class Spikes : MonoBehaviour
    {
        private const float UP_DURATION = 0.3f;
        private const float DOWN_DURATION = 0.3f;
        private const Ease UP_EASE = Ease.InExpo;
        private const Ease DOWN_EASE = Ease.InExpo;

        [SerializeField] private bool _isUp = true;
        [SerializeField] private float _timerShift;
        [SerializeField] private float _damage;
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _spikesHolder;
        [SerializeField] private float _hiddenTime;
        [SerializeField] private float _shownTime;
        [SerializeField] private Vector3 _up = new(0.5f, 0f, -1f);
        [SerializeField] private Vector3 _down = new(0.5f, -1f, -1f);
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _upClip;
        [SerializeField] private AudioClip _downClip;

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
                    _spikesHolder.DOLocalMove(_up, UP_DURATION)
                        .SetEase(UP_EASE)
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

                            _source.PlayOneShot(_upClip);
                        });
                }
                else
                {
                    _spikesHolder.DOLocalMove(_down, DOWN_DURATION)
                        .SetEase(DOWN_EASE)
                        .OnComplete(() =>
                        {
                            _collider.enabled = false;

                            foreach (var captured in _captured)
                            {
                                captured.enabled = true;
                                captured.ResetSpeeds();
                            }
                            _captured.Clear();
                        });
                }
            }
        }
    } 
}

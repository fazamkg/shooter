using UnityEngine;

namespace Faza
{
    public class FootstepSounds : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private AudioClip[] _stepClips;
        [SerializeField] private AudioSource _source;
        [SerializeField] private float _factor = 1f;
        [SerializeField] private float _cap = 0.2f;

        private float _timer;
        private float _cooldown;

        private void Update()
        {
            var speed = _character.HorizontalSpeed;
            _timer += Time.deltaTime;

            if (speed < _cap) return;
            if (_timer < _factor / speed) return;

            _source.PlayOneShot(_stepClips.GetRandom());
            _timer = 0f;
        }
    } 
}

using UnityEngine;

namespace Faza
{
    public class Gib : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip[] _clips;

        private void OnCollisionEnter(Collision collision)
        {
            _source.PlayOneShot(_clips.GetRandom());
        }
    } 
}

using UnityEngine;

namespace Faza
{
    public static class FazaAudio
    {
        private const string AUDIO_SOURCE_PREFAB_PATH = "AudioSourcePrefab";

        public static AudioSource Play(AudioClip clip, float volume = 1f,
            bool spacial = false, float pitch = 1f)
        {
            var source = Resources.Load<AudioSource>(AUDIO_SOURCE_PREFAB_PATH);
            var newObject = Object.Instantiate(source);
            newObject.clip = clip;
            newObject.volume = volume;
            newObject.spatialBlend = spacial ? 1f : 0f;
            newObject.pitch = pitch;
            newObject.Play();
            return newObject;
        }

        public static AudioSource Play(string clip, float volume = 1f,
            bool spacial = false, float pitch = 1f)
        {
            return Play(Resources.Load<AudioClip>(clip), volume, spacial, pitch);
        }
    } 
}

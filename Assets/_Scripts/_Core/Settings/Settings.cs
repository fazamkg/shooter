using System;

namespace Faza
{
    public static class Settings
    {
        public static event Action OnAudioEnabledChanged;

        public static bool AudioEnabled
        {
            get => Storage.GetBool(StorageKey.AUDIO_ENABLED, true);
            set
            {
                Storage.SetBool(StorageKey.AUDIO_ENABLED, value);
                OnAudioEnabledChanged?.Invoke();
            }
        }
    } 
}

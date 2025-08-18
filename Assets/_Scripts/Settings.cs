using System;

namespace Faza
{
    public static class Settings
    {
        public static event Action OnAudioEnabledChanged;

        public static bool AudioEnabled
        {
            get => Storage.GetBool("faza_audio", true);
            set
            {
                Storage.SetBool("faza_audio", value);
                OnAudioEnabledChanged?.Invoke();
            }
        }
    } 
}

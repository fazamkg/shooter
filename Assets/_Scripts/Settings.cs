using System;

namespace Faza
{
    public static class Settings
    {
        public static event Action OnIsLeftyChanged;

        public static bool IsLefty
        {
            get => Storage.GetBool("faza_is_lefty");
            set
            {
                Storage.SetBool("faza_is_lefty", value);
                OnIsLeftyChanged?.Invoke();
            }
        }
    } 
}

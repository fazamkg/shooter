using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Faza
{
    public class ShootingTapArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static event Action OnDown;
        public static event Action OnUp;

        public static bool IsDown { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsDown = true;
            OnDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsDown = false;
            OnUp?.Invoke();
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Faza
{
    public class ShootingTapArea : MonoBehaviour, IPointerDownHandler
    {
        public static event Action OnTap;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnTap?.Invoke();
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

namespace Faza
{
    public class FazaButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            FazaAudio.Play("PC Keyboard_Keystroke_28", 0.3f);

            transform.DOKill();
            transform.DOScale(0.8f, 0.15f).SetEase(Ease.OutCubic);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(1f, 0.05f).SetEase(Ease.InQuart);

            OnUp?.Invoke();
        }
    }
}

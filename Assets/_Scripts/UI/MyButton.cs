using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

namespace Faza
{
    public class MyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler,
        IPointerExitHandler
    {
        public event Action OnUp;

        private bool _inside;

        public void OnPointerDown(PointerEventData eventData)
        {

            transform.DOKill();
            transform.DOScale(0.8f, 0.15f).SetEase(Ease.OutCubic);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _inside = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _inside = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(1f, 0.05f).SetEase(Ease.InQuart);

            if (_inside)
            {
                OnUp?.Invoke();
            }
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

namespace Faza
{
    public class FazaButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const float DOWN_AUDIO_VOLUME = 0.3f;
        private const Ease DOWN_EASE = Ease.OutCubic;
        private const Ease UP_EASE = Ease.InQuart;
        private const float DOWN_DURATION = 0.15f;
        private const float UP_DURATION = 0.05f;
        private const float DOWN_SCALE = 0.8f;

        public event Action OnUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            FazaAudio.Play(AudioKey.KEYSTROKE, DOWN_AUDIO_VOLUME);

            transform.DOKill();
            transform.DOScale(DOWN_SCALE, DOWN_DURATION).SetEase(DOWN_EASE);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(1f, UP_DURATION).SetEase(UP_EASE);

            OnUp?.Invoke();
        }
    }
}

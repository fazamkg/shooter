using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Faza
{
    public class TutorialPopView : MonoBehaviour
    {
        private const float APPEAR_DURATION = 0.9f;
        private const Ease APPEAR_EASE = Ease.OutBack;
        private const float DISAPPEAR_DURATION = 0.4f;
        private const Ease DISAPPEAR_EASE = Ease.InBack;
        private const float WIDTH = 760f;
        private const float HEIGHT = 280f;

        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioSource _source;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text[] _texts;

        public void Init(string input)
        {
            foreach (var text in _texts)
            {
                text.text = input;
            }

            _rectTransform.sizeDelta = Vector2.zero;
        }

        public Tween Appear(Vector2 pos)
        {
            _source.PlayOneShot(_clip);
            transform.position = pos;
            return _rectTransform.DOSizeDelta(new(WIDTH, HEIGHT), APPEAR_DURATION).SetEase(APPEAR_EASE);
        }

        public Tween Disappear()
        {
            return _rectTransform.DOSizeDelta(new(0f, 0f), DISAPPEAR_DURATION).SetEase(DISAPPEAR_EASE);
        }
    } 
}

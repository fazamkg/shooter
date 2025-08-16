using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Faza
{
    public class TutorialPopView : MonoBehaviour
    {
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
            transform.position = pos;
            return _rectTransform.DOSizeDelta(new(630f, 180f), 0.9f).SetEase(Ease.OutBack);
        }

        public Tween Disappear()
        {
            return _rectTransform.DOSizeDelta(new(0f, 0f), 0.4f).SetEase(Ease.InBack);
        }
    } 
}

using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class InfoButtonView : MonoBehaviour
    {
        private const float TWEEN_DURATION = 0.3f;
        private const Ease OPEN_EASE = Ease.OutBack;
        private const Ease CLOSE_EASE = Ease.InBack;

        [SerializeField] private FazaButtonView _button;
        [SerializeField] private RectTransform _pop;

        private bool _opened;

        private void Awake()
        {
            _button.OnUp += Button_OnUp;
            _pop.localScale = Vector3.zero;
        }

        private void Update()
        {
            if (_opened == false) return;

            if (Input.GetMouseButtonDown(0))
            {
                _pop.DOKill();
                _pop.DOScale(0f, TWEEN_DURATION).SetEase(CLOSE_EASE);
                _opened = false;
            }
        }

        private void Button_OnUp()
        {
            if (_opened == false)
            {
                _pop.DOKill();
                _pop.DOScale(1f, TWEEN_DURATION).SetEase(OPEN_EASE);
                _opened = true;
            }
            else
            {
                _pop.DOKill();
                _pop.DOScale(0f, TWEEN_DURATION).SetEase(CLOSE_EASE);
                _opened = false;
            }
        }
    }
}

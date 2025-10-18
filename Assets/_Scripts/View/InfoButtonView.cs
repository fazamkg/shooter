using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class InfoButtonView : MonoBehaviour
    {
        [SerializeField] private MyButtonView _button;
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
                _pop.DOScale(0f, 0.3f).SetEase(Ease.InBack);
                _opened = false;
            }
        }

        private void Button_OnUp()
        {
            if (_opened == false)
            {
                _pop.DOKill();
                _pop.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
                _opened = true;
            }
            else
            {
                _pop.DOKill();
                _pop.DOScale(0f, 0.3f).SetEase(Ease.InBack);
                _opened = false;
            }
        }
    }
}

using DG.Tweening;
using UnityEngine;

namespace Faza
{
    public class BoosterChoicePanel : MonoBehaviour
    {
        [SerializeField] private MyButton _closeButton;
        [SerializeField] private MyButton _mainButton;
        [SerializeField] private MyButton _altButton;

        private void Awake()
        {
            _closeButton.OnUp += CloseButton_OnUp;

            transform.localScale = Vector3.zero;
        }

        private void CloseButton_OnUp()
        {
            Disappear();
        }

        public void Appear(BoosterData booster)
        {
            transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }

        public void Disappear()
        {
            transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
        }
    } 
}

using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class PistolAnimation : AbstractAnimation
    {
        [SerializeField] private Transform _root;
        [SerializeField] private Transform _hammer;
        [SerializeField] private Transform _magazin;
        [SerializeField] private Transform _slide;
        [SerializeField] private Transform _trigger;
        [SerializeField] private float _rotationX = 17.64f;
        [SerializeField] private float _rotationDuration = 0.3f;
        [SerializeField] private float _rotationDuration2 = 0.4f;

        public override void Play()
        {
            var seq = DOTween.Sequence();

            var to = new Vector3(_rotationX, 0f, 0f);

            seq.Append(_root.DOLocalRotate(to, _rotationDuration).SetEase(Ease.Linear));
            seq.Append(_root.DOLocalRotate(Vector3.zero, _rotationDuration2).SetEase(Ease.Linear));

            seq.SetEase(Ease.OutCirc);
        }
    } 
}

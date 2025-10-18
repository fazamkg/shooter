using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class SkullView : MonoBehaviour
    {
        [SerializeField] private RectTransform _cross;

        public bool CrossedOut { get; private set; }

        public void CrossOut()
        {
            CrossedOut = true;
            _cross.localScale = Vector3.one;
        }
    } 
}

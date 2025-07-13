using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Faza
{
    public class UpgradeScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform _title;
        [SerializeField] private MyButton _nextButton;
        [SerializeField] private CanvasGroup _group;

        private void Awake()
        {
            _group.alpha = 0f;
            _group.blocksRaycasts = false;

            _nextButton.OnUp += NextButton_OnUp;
        }

        private void NextButton_OnUp()
        {
            _group.blocksRaycasts = false;

            var seq = DOTween.Sequence();

            seq.Append(_group.DOFade(0f, 0.5f).SetEase(Ease.InOutCirc));
            seq.Append(_title.DOScale(0f, 0.3f).SetEase(Ease.InBack));
            seq.Append(_nextButton.transform.DOScale(0f, 0.3f).SetEase(Ease.InOutBack));
            seq.SetEase(Ease.Linear);
        }

        public void Appear(UpgradeGroupData[] groups)
        {
            _group.blocksRaycasts = true;
            _title.localScale = Vector3.zero;
            _nextButton.transform.localScale = Vector3.zero;

            var seq = DOTween.Sequence();

            seq.AppendInterval(1f);
            seq.Append(_group.DOFade(1f, 0.5f).SetEase(Ease.InOutCirc));
            seq.Append(_title.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
            seq.AppendInterval(0.3f);
            seq.Append(_nextButton.transform.DOScale(1f, 0.3f).SetEase(Ease.InOutBack));
            seq.SetEase(Ease.Linear);
        }
    } 
}

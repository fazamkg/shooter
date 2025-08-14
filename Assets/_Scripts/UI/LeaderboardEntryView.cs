using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Faza
{
    public class LeaderboardEntryView : MonoBehaviour
    {
        [SerializeField] private RectTransform _specular;
        [SerializeField] private float _speed;
        [SerializeField] private TMP_Text[] _rankTexts;
        [SerializeField] private TMP_Text[] _nameTexts;
        [SerializeField] private TMP_Text[] _timeTexts;

        private void Update()
        {
            var pos = Mathf.Lerp(-90f, 1500f, (Time.time * _speed).Frac());
            _specular.SetAnchorPosX(pos);
        }
    } 
}

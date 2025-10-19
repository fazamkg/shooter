using UnityEngine;
using TMPro;

namespace Faza
{
    public class LeaderboardEntryView : MonoBehaviour
    {
        private const string TIME_FORMAT = @"mm\:ss\.fff";
        private const float SHINE_FROM_POS = -90f;
        private const float SHINE_TO_POS = 1500f;

        [SerializeField] private RectTransform _specular;
        [SerializeField] private float _speed;
        [SerializeField] private TMP_Text[] _rankTexts;
        [SerializeField] private TMP_Text[] _nameTexts;
        [SerializeField] private TMP_Text[] _timeTexts;

        public void Init(LeaderboardEntry entry)
        {
            var rank = entry.Rank.ToString();
            var name = entry.Name;
            var time = entry.Time.ToString(TIME_FORMAT);

            foreach (var text in _rankTexts)
            {
                text.text = rank;
            }

            foreach (var text in _nameTexts)
            {
                text.text = name;
            }

            foreach (var text in _timeTexts)
            {
                text.text = time;
            }
        }

        private void Update()
        {
            var pos = Mathf.Lerp(SHINE_FROM_POS, SHINE_TO_POS, (Time.time * _speed).Frac());
            _specular.SetAnchorPosX(pos);
        }
    } 
}

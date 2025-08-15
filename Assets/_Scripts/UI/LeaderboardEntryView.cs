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

        public void Init(LeaderboardEntry entry)
        {
            var rank = entry.Rank.ToString();
            var name = entry.Name;
            var time = entry.Time.ToString(@"mm\:ss\.fff");

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
            var pos = Mathf.Lerp(-90f, 1500f, (Time.time * _speed).Frac());
            _specular.SetAnchorPosX(pos);
        }
    } 
}

using UnityEngine;
using TMPro;

namespace Faza
{
    public class LevelButtonView : MonoBehaviour
    {
        private const string TIME_FORMAT = @"mm\:ss\.fff";

        [SerializeField] private FazaButtonView _button;
        [SerializeField] private TMP_Text[] _texts;
        [SerializeField] private TMP_Text[] _timespanTexts;
        [SerializeField] private TMP_Text[] _rankTexts;

        private LevelData _levelData;

        public void Init(LevelData levelData)
        {
            _levelData = levelData;

            _button.OnUp += Button_OnUp;

            var number = levelData.name.GetNumberPart();
            foreach (var text in _texts)
            {
                text.text = number;
            }

            var time = levelData.GetCompletedTimespan().ToString(TIME_FORMAT);
            foreach (var text in _timespanTexts)
            {
                text.text = time;
            }

            var rank = levelData.GetPlayerRank();
            var rankText = rank == -1 ? "" : rank.ToString();
            foreach (var text in _rankTexts)
            {
                text.text = rankText;
            }
        }

        private void Button_OnUp()
        {
            LevelManager.Instance.LoadLevel(_levelData);
        }
    } 
}

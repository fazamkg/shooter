using UnityEngine;
using TMPro;

namespace Faza
{
    public class LevelButtonView : MonoBehaviour
    {
        [SerializeField] private MyButton _button;
        [SerializeField] private TMP_Text[] _texts;
        [SerializeField] private TMP_Text[] _timespanTexts;

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

            var time = levelData.GetCompletedTimespan().ToString(@"hh\:mm\:ss");
            foreach (var text in _timespanTexts)
            {
                text.text = time;
            }
        }

        private void Button_OnUp()
        {
            LevelManager.Instance.LoadLevel(_levelData);
        }
    } 
}

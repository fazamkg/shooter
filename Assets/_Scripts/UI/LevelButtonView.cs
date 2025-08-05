using UnityEngine;
using TMPro;

namespace Faza
{
    public class LevelButtonView : MonoBehaviour
    {
        [SerializeField] private MyButton _button;
        [SerializeField] private TMP_Text[] _texts;

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
        }

        private void Button_OnUp()
        {
            LevelManager.Instance.LoadLevel(_levelData);
        }
    } 
}

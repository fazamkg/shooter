using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Faza
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _texts;

        private void Awake()
        {
            var levelNumber = SceneManager.GetActiveScene().name.GetNumberPart();

            var text = $"УРОВЕНЬ {levelNumber}";

            foreach (var tmp in _texts)
            {
                tmp.text = text;
            }
        }
    } 
}

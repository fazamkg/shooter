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
            var level = SceneManager.GetActiveScene().buildIndex;

            var text = $"УРОВЕНЬ {level}";

            foreach (var tmp in _texts)
            {
                tmp.text = text;
            }
        }
    } 
}

using UnityEngine;
using UnityEngine.UI;

namespace Faza
{
    public class GameInit : MonoBehaviour
    {
        [SerializeField] private Image _bg;
        [SerializeField] private MyButtonView _start;
        [SerializeField] private float _speed;

        private void Awake()
        {
            LevelManager.Instance.LoadLevelFromSave();
        }

        private void Update()
        {
            _bg.color = _bg.color.AddHue(Time.deltaTime * _speed);
        }

        public void OnLevelCompleted(LevelData level)
        {

        }
    } 
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Faza
{
    public class GameInit : MonoBehaviour
    {
        [SerializeField] private Image _bg;
        [SerializeField] private Button _start;
        [SerializeField] private float _speed;

        private void Awake()
        {
            _start.onClick.AddListener(OnStart);
        }

        private void Update()
        {
            _bg.color = _bg.color.AddHue(Time.deltaTime * _speed);
        }

        private void OnStart()
        {
            SceneManager.LoadScene(1);
        }
    } 
}

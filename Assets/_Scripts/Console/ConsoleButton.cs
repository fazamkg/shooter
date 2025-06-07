using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Faza
{
    public class ConsoleButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        private Action _action;

        public void Init(string name, Action action)
        {
            _text.text = name;
            _action = action;
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _action();
        }
    } 
}

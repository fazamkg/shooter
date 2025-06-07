using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Faza
{
    public class Console : MonoBehaviour
    {
        [SerializeField] private Button _toggleButton;
        [SerializeField] private ConsoleButton _buttonPrefab;
        [SerializeField] private Transform _buttonParent;

        private static Console _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var consoleCanvas = Resources.Load<GameObject>("ConsoleCanvas");
            _instance = Instantiate(consoleCanvas).GetComponent<Console>();
            DontDestroyOnLoad(_instance.gameObject);
        }

        private void Awake()
        {
            _toggleButton.onClick.AddListener(OnClickToggle);
            _buttonParent.gameObject.SetActive(false);
        }

        private void OnClickToggle()
        {
            _buttonParent.gameObject.Toggle();
        }

        private void AddButtonInternal(string name, Action action)
        {
            var button = Instantiate(_buttonPrefab, _buttonParent);
            button.Init(name, action);
        }

        public static void AddButton(string name, Action action)
        {
            _instance.AddButtonInternal(name, action);
        }

        public static Coroutine StartCoroutine_(IEnumerator routine)
        {
            return _instance.StartCoroutine(routine);
        }

        public static void StopCoroutine_(IEnumerator routine)
        {
            _instance.StopCoroutine(routine);
        }
    } 
}

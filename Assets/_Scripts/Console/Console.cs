using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace Faza
{
    public delegate void Command(params string[] args);

    public class Console : MonoBehaviour
    {
        [SerializeField] private Button _toggleButton;
        [SerializeField] private ConsoleButton _buttonPrefab;
        [SerializeField] private Transform _buttonParent;
        [SerializeField] private GameObject _toggleObject;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_Text _console;

        private Dictionary<string, Command> _commands = new();
        private Dictionary<KeyCode, string> _binds = new();
        private bool _isReadingBinds;

        private static Console _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var consoleCanvas = Resources.Load<GameObject>("ConsoleCanvas");
            _instance = Instantiate(consoleCanvas).GetComponent<Console>();
            DontDestroyOnLoad(_instance.gameObject);

            AddCommand("bind", (args) => Bind(args[0].ToKeyCode(), args[1]));
        }

        private void Awake()
        {
            _toggleButton.onClick.AddListener(OnClickToggle);
            _toggleObject.SetActive(false);
            _inputField.onEndEdit.AddListener(OnCommandEntered);
        }

        private void Update()
        {
            if (_isReadingBinds == false) return;

            foreach (var bind in _binds)
            {
                if (Input.GetKeyDown(bind.Key))
                {
                    ExecuteExpressionInternal(bind.Value);
                }
            }
        }

        private void OnClickToggle()
        {
            _toggleObject.Toggle();
        }

        private void OnCommandEntered(string text)
        {
            _inputField.text = "";
            ExecuteExpressionInternal(text);
        }

        private void ExecuteExpressionInternal(string expression)
        {
            if (string.IsNullOrEmpty(expression)) return;

            var parts = expression.Split(' ');
            var key = parts[0];

            var exist = _commands.TryGetValue(key, out var command);
            if (exist)
            {
                try
                {
                    command(parts.Skip(1).ToArray());
                }
                catch (Exception e)
                {
                    Log($"Command '{key}' caught exception: '{e}'!");
                }
            }
            else
            {
                Log($"Command '{key}' does not exist!");
            }
        }

        private void StartReadingBindsInternal()
        {
            _isReadingBinds = true;
        }

        private void StopReadingBindsInteral()
        {
            _isReadingBinds = false;
        }

        private void OpenConsoleInternal()
        {
            _toggleObject.SetActive(true);
        }

        private void CloseConsoleInternal()
        {
            _toggleObject.SetActive(false);
        }

        private void AddButtonInternal(string name, Action action)
        {
            var button = Instantiate(_buttonPrefab, _buttonParent);
            button.Init(name, action);
        }

        private void AddCommandInternal(string name, Command action)
        {
            _commands[name] = action;
        }

        private void BindInternal(KeyCode keyCode, string expression)
        {
            _binds[keyCode] = expression;
        }

        private void LogInternal(string message)
        {
            Debug.Log(message);
            _console.text += $"{message}\n";
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

        public static void AddCommand(string name, Command action)
        {
            _instance.AddCommandInternal(name, action);
        }

        public static void Log(string message)
        {
            _instance.LogInternal(message);
        }

        public static void Bind(KeyCode keyCode, string expression)
        {
            _instance.BindInternal(keyCode, expression);
        }

        public static void StartReadingBinds()
        {
            _instance.StartReadingBindsInternal();
        }

        public static void StopReadingBinds()
        {
            _instance.StopReadingBindsInteral();
        }

        public static void OpenConsole()
        {
            _instance.OpenConsoleInternal();
        }

        public static void CloseConsole()
        {
            _instance.CloseConsoleInternal();
        }

        public static void PlayOneFrame()
        {
            IEnumerator playOneFrame()
            {
                Time.timeScale = 1f;
                yield return null;
                Time.timeScale = 0f;
            }
            StartCoroutine_(playOneFrame());
        }
    } 
}

using UnityEngine;

namespace Faza
{
    public class PlayerInput : CharacterInput
    {
        [SerializeField] private float _sensitivity;
        [SerializeField] private GameObject _camera;
        [SerializeField] private float _turningCap;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Health _health;
        [SerializeField] private Shooter _shooter;
        [SerializeField] private Character _character;
        [SerializeField] private GameObject _critGlow;
        [SerializeField] private GameObject _hasteGlow;
        [SerializeField] private GameObject _armorGlow;

        private bool _locked;

        public GameObject Camera => _camera;
        public Health Health => _health;
        public Shooter Shooter => _shooter;
        public Character Character => _character;
        public CharacterAnimator CharacterAnimator => GetComponentInChildren<CharacterAnimator>();
        public static PlayerInput Instance { get; private set; }

        private void Awake()
        {
            _locked = true;
            Console.StartReadingBinds();

            Instance = this;

            _health.OnDeath += Health_OnDeath;
        }

        private void Health_OnDeath()
        {
            DeactivateArmorGlow();
            DeactivateCritGlow();
            DeactivateHasteGlow();
        }

        private void Start()
        {
            CameraTracker.Activate("main");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _locked = !_locked;

                if (_locked)
                {
                    Time.timeScale = 1f;
                    Console.StartReadingBinds();
                    Console.CloseConsole();
                }
                else
                {
                    Time.timeScale = 0f;
                    Console.StopReadingBinds();
                    Console.OpenConsole();
                }
            }
        }

        public override float GetCameraX()
        {
            if (_shooter.WithinShooting) return 0f;
            if (_health.IsDead) return 0f;
            if (_locked == false) return 0f;

            var joyV = Joystick.GetInput("move").y;
            var kbV = Input.GetAxisRaw("Vertical");
            var vertical = Mathf.Min(1f, joyV + kbV);

            var joyH = Joystick.GetInput("move").x;
            var kbH = Input.GetAxisRaw("Horizontal");
            var horizontal = Mathf.Min(1f, joyH + kbH);

            var lookForward = _camera.transform.forward;
            var direction = new Vector3(horizontal, 0f, vertical).normalized;

            var cross = Vector3.Cross(lookForward, direction);
            var dot = Vector3.Dot(lookForward, direction);
            var result = 0f;

            if (dot < -0.95f)
            {
                result = -1f;
            }
            else
            {
                result = cross.y.Abs() < _turningCap ? 0f : cross.y;
            }

            return result * Time.deltaTime * _rotationSpeed;
        }

        public override float GetCameraY()
        {
            return 0f;
        }

        public Vector3 GetRawMove()
        {
            if (_shooter.WithinShooting) return Vector3.zero;
            if (_health.IsDead) return Vector3.zero;
            if (_locked == false) return Vector3.zero;

            var kbV = Input.GetAxisRaw("Vertical");
            var kbH = Input.GetAxisRaw("Horizontal");
            var keyboard = new Vector3(kbH, 0f, kbV).normalized;

            var joy = Joystick.GetInput("move");

            var h = Mathf.Clamp(keyboard.x + joy.x, -1f, 1f);
            var v = Mathf.Clamp(keyboard.z + joy.y, -1f, 1f);
            var input = new Vector3(h, 0f, v);

            return input;
        }

        public override Vector3 GetMove()
        {
            if (_shooter.WithinShooting) return Vector3.zero;
            if (_health.IsDead) return Vector3.zero;
            if (_locked == false) return Vector3.zero;

            var kbV = Input.GetAxisRaw("Vertical");
            var kbH = Input.GetAxisRaw("Horizontal");
            var keyboard = new Vector3(kbH, 0f, kbV).normalized;

            var joy = Joystick.GetInput("move");

            var h = Mathf.Clamp(keyboard.x + joy.x, -1f, 1f);
            var v = Mathf.Clamp(keyboard.z + joy.y, -1f, 1f);
            var input = new Vector3(h, 0f, v);

            var lookRight = _camera.transform.right;
            var lookForward = _camera.transform.forward;

            var hDot = Vector3.Dot(lookRight, input);
            var vDot = Vector3.Dot(lookForward, input);

            return new Vector3(hDot, 0f, vDot);
        }

        public override bool GetJump()
        {
            return false;
        }

        public override bool GetUse()
        {
            return false;
        }

        public override bool IsFire()
        {
            return false;
        }

        public void ActivateCritGlow()
        {
            _critGlow.SetActive(true);
        }

        public void DeactivateCritGlow()
        {
            _critGlow.SetActive(false);
        }

        public void ActivateHasteGlow()
        {
            _hasteGlow.SetActive(true);
        }

        public void DeactivateHasteGlow()
        {
            _hasteGlow.SetActive(false);
        }

        public void ActivateArmorGlow()
        {
            _armorGlow.SetActive(true);
        }

        public void DeactivateArmorGlow()
        {
            _armorGlow.SetActive(false);
        }
    }
}

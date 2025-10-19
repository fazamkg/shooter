using UnityEngine;
using UnityEngine.SceneManagement;

namespace Faza
{
    public class EntryPoint : MonoBehaviour
    {
        private const string PLAYED_ID = "player";

        [SerializeField] private GameObject[] _uis;
        [SerializeField] private SkinnedMeshRenderer _playerSMR;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private UpgradeGroupData[] _allUpgradeGroups;
        [SerializeField] private Sprite _coinSprite;
        [SerializeField] private Sprite _rvSprite;

        private Waypoint _from;
        private Waypoint _to;
        private EnemyInput _enemy;
        private Vector3 _position;

        public static bool IsDebugOn { get; private set; }

        private void Awake()
        {
            BoosterData.Init();

            Input.multiTouchEnabled = false;
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount = 1;

            Console.AddCommand(CommandKey.ENEMY_CAMERA_X, (args) =>
            {
                GetAtCrosshair<EnemyInput>().SetCameraX(args[0].ToFloat());
            });
            Console.AddCommand(CommandKey.ENEMY_HORIZONTAL, (args) =>
            {
                GetAtCrosshair<EnemyInput>().SetHorizontal(args[0].ToFloat());
            });
            Console.AddCommand(CommandKey.ENEMY_VERTICAL, (args) =>
            {
                GetAtCrosshair<EnemyInput>().SetVertical(args[0].ToFloat());
            });
            Console.AddCommand(CommandKey.MAIN_CAMERA, (args) =>
            {
                var player = Tracker.Get<PlayerInput>(PLAYED_ID);
                player.Camera.ActivateSingle();
            });
            Console.AddCommand(CommandKey.ENEMY_CAMERA_FP, (args) =>
            {
                GetAtCrosshair<EnemyInput>().FpCamera.ActivateSingle();
            });
            Console.AddCommand(CommandKey.ENEMY_CAMERA_TP, (args) =>
            {
                GetAtCrosshair<EnemyInput>().TpCamera.ActivateSingle();
            });
            Console.AddCommand(CommandKey.ENEMY_JUMP, (args) =>
            {
                GetAtCrosshair<EnemyInput>().SetJump(args[0].ToBool());
            });
            Console.AddCommand(CommandKey.ENEMY_STOP_ALL, (args) =>
            {
                var enemies = FindObjectsByType<EnemyInput>(FindObjectsSortMode.None);
                foreach (var enemy in enemies)
                {
                    enemy.SetWaypoint(null);
                }
            });
            Console.AddCommand(CommandKey.ENEMY_SWITCH_ANIMATION_SET, (args) =>
            {
                var name = GetAtCrosshair<CharacterAnimator>().IncrementAnimationSet();
                Console.Log(name);
            });

            Console.AddCommand(CommandKey.TIME_SCALE, (args) => Time.timeScale = args[0].ToFloat());
            Console.AddCommand(CommandKey.ONE_FRAME, (args) => Console.PlayOneFrame());

            Console.AddCommand(CommandKey.WAYPOINT, CreateWaypoint);
            Console.AddCommand(CommandKey.LOOP_WAYPOINT, (args) => Waypoint.LoopLastWaypoint());
            Console.AddCommand(CommandKey.ASSIGN_WAYPOINT, (args) =>
            {
                var enemy = GetAtCrosshair<EnemyInput>();
                var lastWaypoint = Waypoint.LastCreatedWaypoint;
                var start = lastWaypoint.GetStart();
                enemy.SetWaypoint(start);
            });
            Console.AddCommand(CommandKey.SHOW_WAYPOINTS, (args) => Waypoint.ShowAll());
            Console.AddCommand(CommandKey.HIDE_WAYPOINTS, (args) => Waypoint.HideAll());

            Console.AddCommand(CommandKey.TARGET_ACCELERATION, (args) =>
            {
                GetAtCrosshair<Character>().SetAccelration(args[0].ToFloat());
            });
            Console.AddCommand(CommandKey.TARGET_FRICTION, (args) =>
            {
                GetAtCrosshair<Character>().SetFriction(args[0].ToFloat());
            });
            Console.AddCommand(CommandKey.TARGET_MAXSPEED, (args) =>
            {
                GetAtCrosshair<Character>().SetMaxSpeed(args[0].ToFloat());
            });
            Console.AddCommand(CommandKey.PLAYER_SCALED_TIME, (args) =>
            {
                var player = Tracker.Get<Character>(PLAYED_ID);
                player.DeltaTimeScaled = args[0].ToBool();
            });
            Console.AddCommand(CommandKey.WP_SET_FROM, (args) =>
            {
                _from = GetAtCrosshair<Waypoint>();
            });
            Console.AddCommand(CommandKey.WP_SET_TO, (args) =>
            {
                _to = GetAtCrosshair<Waypoint>();
            });
            Console.AddCommand(CommandKey.WP_FIND_PATH, (args) =>
            {
                var path = _from.FindPath(_to);
                foreach (var wp in Waypoint.All)
                {
                    wp.IsMarked = false;
                }
                foreach (var wp in path)
                {
                    wp.IsMarked = true;
                }
            });
            Console.AddCommand(CommandKey.SET_ENEMY, (args) =>
            {
                _enemy = GetAtCrosshair<EnemyInput>();
            });
            Console.AddCommand(CommandKey.ENEMY_SET_DESTINATION, (args) =>
            {
                var player = Tracker.Get<Character>(PLAYED_ID);
                var hit = player.GetCrosshairInfo(out var hitInfo);
                if (hit)
                {
                    _enemy.SetDestination(hitInfo.point);
                }
            });
            Console.AddCommand(CommandKey.SET_POS, (args) =>
            {
                var player = Tracker.Get<Character>(PLAYED_ID);
                var hit = player.GetCrosshairInfo(out var hitInfo);
                if (hit)
                {
                    _position = hitInfo.point;
                }
            });
            Console.AddCommand(CommandKey.DEBUG, (args) =>
            {
                IsDebugOn = !IsDebugOn;
            });
            Console.AddCommand(CommandKey.NOCLIP, (args) =>
            {
                var player = Tracker.Get<Character>(PLAYED_ID);
                player.IsNoclip = !player.IsNoclip;
            });
            Console.AddCommand(CommandKey.DESTROY, (args) =>
            {
                Destroy(GetAtCrosshair<Transform>().gameObject);
            });
            Console.AddCommand(CommandKey.RELOAD_SCENE, (args) =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
            Console.AddCommand(CommandKey.CLEAR_LINES, (args) => Line.ClearLines());
            Console.AddCommand(CommandKey.MAX_FPS, (args) => Application.targetFrameRate = args[0].ToInt());
            Console.AddCommand(CommandKey.KILL_ALL_ENEMIES, (args) =>
            {
                var enemies = FindObjectsByType<EnemyInput>(FindObjectsSortMode.None);
                foreach (var enemy in enemies)
                {
                    enemy.Health.TakeDamage(100_000f, Vector3.zero);
                }
            });
            Console.AddCommand(CommandKey.ADD_MONEY, (args) => Currency.AddCoins(args[0].ToFloat(), Vector2.zero));

            Console.Bind(KeyCode.Q, CommandKey.WAYPOINT);
            Console.Bind(KeyCode.R, CommandKey.LOOP_WAYPOINT);
            Console.Bind(KeyCode.T, CommandKey.ASSIGN_WAYPOINT);
            Console.Bind(KeyCode.J, $"{CommandKey.WAYPOINT} jump");
            Console.Bind(KeyCode.Z, CommandKey.ENEMY_SWITCH_ANIMATION_SET);
            Console.Bind(KeyCode.X, CommandKey.ENEMY_STOP_ALL);
            Console.Bind(KeyCode.V, CommandKey.SET_ENEMY);
            Console.Bind(KeyCode.B, CommandKey.ENEMY_SET_DESTINATION);

            Console.AddButton("low", () =>
            {
                QualitySettings.SetQualityLevel(0);
            });
            Console.AddButton("med", () =>
            {
                QualitySettings.SetQualityLevel(1);
            });
            Console.AddButton("high", () =>
            {
                QualitySettings.SetQualityLevel(2);
            });
            Console.AddButton("toggle ui", () =>
            {
                foreach (var ui in _uis)
                {
                    ui.Toggle();
                }
            });
            Console.AddButton("toggle enemies", () =>
            {
                var enemies = FindObjectsByType<EnemyInput>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var enemy in enemies)
                {
                    enemy.gameObject.Toggle();
                }
            });
            Console.AddButton("swap material", () =>
            {
                var things = FindObjectsByType<MaterialSwap>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var thing in things)
                {
                    var rend = thing.GetComponent<MeshRenderer>();

                    var mat = rend.sharedMaterial;
                    if (mat.name == "FazaDefault")
                    {
                        rend.sharedMaterial = Resources.Load<Material>("DefaultUnlit");
                    }
                    else
                    {
                        rend.sharedMaterial = Resources.Load<Material>("FazaDefault");
                    }
                }
            });
            Console.AddButton("toggle cam", () =>
            {
                var cams = FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var cam in cams)
                {
                    if (cam.gameObject.name == "MainCamera")
                    {
                        cam.enabled = !cam.enabled;
                        break;
                    }
                }
            });
            Console.AddButton("toggle player", () =>
            {
                var player = Tracker.Get<Character>("player");
                player.gameObject.Toggle();
            });
            Console.AddButton("toggle player smr", () =>
            {
                _playerSMR.enabled = !_playerSMR.enabled;
            });
            Console.AddButton("toggle player animator", () =>
            {
                _playerAnimator.enabled = !_playerAnimator.enabled;
            });
            Console.AddCommand("win", (args) =>
            {
                var ui = FindFirstObjectByType<UserInterfaceView>();
                ui.Win(10f);
            });
        }

        private void Start()
        {
            foreach (var group in _allUpgradeGroups)
            {
                var upgrades = group.GetPurchasedUpgrades();

                foreach (var upgrade in upgrades)
                {
                    upgrade.Apply();
                }
            }
        }

        private T GetAtCrosshair<T>() where T : Component
        {
            var player = Tracker.Get<Character>(PLAYED_ID);
            player.GetCrosshairInfo(out var hitInfo);

            var root = hitInfo.transform.GetComponent<T>();
            if (root) return root;

            return hitInfo.transform.GetComponentInChildren<T>();
        }

        private void CreateWaypoint(params string[] args)
        {
            var command = args.Length > 0 ? args[0] : "";

            var player = Tracker.Get<Character>(PLAYED_ID);
            var hit = player.GetCrosshairInfo(out var hitInfo);
            if (hit)
            {
                var waypoint = Resources.Load<Waypoint>("Waypoint");
                var pos = hitInfo.point + hitInfo.normal * 0.5f;
                var instance = Instantiate(waypoint, pos, Quaternion.identity);
                instance.Init(command);
            }
        }
    } 
}

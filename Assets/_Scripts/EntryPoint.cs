using UnityEngine;
using UnityEngine.SceneManagement;

namespace Faza
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private GameObject[] _uis;
        [SerializeField] private SkinnedMeshRenderer _playerSMR;
        [SerializeField] private Animator _playerAnimator;

        private Waypoint _from;
        private Waypoint _to;
        private EnemyInput _enemy;
        private Vector3 _position;

        public static bool IsDebugOn { get; private set; }

        private void Awake()
        {
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;

            #region Commands
            Console.AddCommand("enemy_camera_x", (args) =>
            {
                GetAtCrosshair<EnemyInput>().SetCameraX(args[0].ToFloat());
            });
            Console.AddCommand("enemy_horizontal", (args) =>
            {
                GetAtCrosshair<EnemyInput>().SetHorizontal(args[0].ToFloat());
            });
            Console.AddCommand("enemy_vertical", (args) =>
            {
                GetAtCrosshair<EnemyInput>().SetVertical(args[0].ToFloat());
            });
            Console.AddCommand("main_camera", (args) =>
            {
                var player = Tracker.Get<PlayerInput>("player");
                player.Camera.ActivateSingle();
            });
            Console.AddCommand("enemy_camera_fp", (args) =>
            {
                GetAtCrosshair<EnemyInput>().FpCamera.ActivateSingle();
            });
            Console.AddCommand("enemy_camera_tp", (args) =>
            {
                GetAtCrosshair<EnemyInput>().TpCamera.ActivateSingle();
            });
            Console.AddCommand("enemy_jump", (args) =>
            {
                GetAtCrosshair<EnemyInput>().SetJump(args[0].ToBool());
            });
            Console.AddCommand("enemy_stop_all", (args) =>
            {
                var enemies = FindObjectsOfType<EnemyInput>();
                foreach (var enemy in enemies)
                {
                    enemy.SetWaypoint(null);
                }
            });
            Console.AddCommand("enemy_switch_animation_set", (args) =>
            {
                var name = GetAtCrosshair<CharacterAnimator>().IncrementAnimationSet();
                Console.Log(name);
            });

            Console.AddCommand("time_scale", (args) => Time.timeScale = args[0].ToFloat());
            Console.AddCommand("one_frame", (args) => Console.PlayOneFrame());

            Console.AddCommand("waypoint", CreateWaypoint);
            Console.AddCommand("loop_waypoint", (args) => Waypoint.LoopLastWaypoint());
            Console.AddCommand("assign_waypoint", (args) =>
            {
                var enemy = GetAtCrosshair<EnemyInput>();
                var lastWaypoint = Waypoint.LastCreatedWaypoint;
                var start = lastWaypoint.GetStart();
                enemy.SetWaypoint(start);
            });
            Console.AddCommand("show_waypoints", (args) => Waypoint.ShowAll());
            Console.AddCommand("hide_waypoints", (args) => Waypoint.HideAll());

            Console.AddCommand("target_acceleration", (args) =>
            {
                GetAtCrosshair<Character>().SetAccelration(args[0].ToFloat());
            });
            Console.AddCommand("target_friction", (args) =>
            {
                GetAtCrosshair<Character>().SetFriction(args[0].ToFloat());
            });
            Console.AddCommand("target_maxspeed", (args) =>
            {
                GetAtCrosshair<Character>().SetMaxSpeed(args[0].ToFloat());
            });
            Console.AddCommand("player_scaled_time", (args) =>
            {
                var player = Tracker.Get<Character>("player");
                player.DeltaTimeScaled = args[0].ToBool();
            });
            Console.AddCommand("wp_set_from", (args) =>
            {
                _from = GetAtCrosshair<Waypoint>();
            });
            Console.AddCommand("wp_set_to", (args) =>
            {
                _to = GetAtCrosshair<Waypoint>();
            });
            Console.AddCommand("wp_find_path", (args) =>
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
            Console.AddCommand("set_enemy", (args) =>
            {
                _enemy = GetAtCrosshair<EnemyInput>();
            });
            Console.AddCommand("enemy_set_destination", (args) =>
            {
                var player = Tracker.Get<Character>("player");
                var hit = player.GetCrosshairInfo(out var hitInfo);
                if (hit)
                {
                    _enemy.SetDestination(hitInfo.point);
                }
            });
            Console.AddCommand("set_pos", (args) =>
            {
                var player = Tracker.Get<Character>("player");
                var hit = player.GetCrosshairInfo(out var hitInfo);
                if (hit)
                {
                    _position = hitInfo.point;
                }
            });
            Console.AddCommand("debug", (args) =>
            {
                IsDebugOn = !IsDebugOn;
            });
            Console.AddCommand("noclip", (args) =>
            {
                var player = Tracker.Get<Character>("player");
                player.IsNoclip = !player.IsNoclip;
            });
            Console.AddCommand("destroy", (args) =>
            {
                Destroy(GetAtCrosshair<Transform>().gameObject);
            });
            Console.AddCommand("reload_scene", (args) =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
            Console.AddCommand("clear_lines", (args) => Line.ClearLines());
            Console.AddCommand("max_fps", (args) => Application.targetFrameRate = args[0].ToInt());
            Console.AddCommand("kill_all_enemies", (args) =>
            {
                var enemies = FindObjectsOfType<EnemyInput>();
                foreach (var enemy in enemies)
                {
                    enemy.Health.TakeDamage(100_000f, Vector3.zero);
                }
            });

            Console.Bind(KeyCode.Q, "waypoint");
            Console.Bind(KeyCode.R, "loop_waypoint");
            Console.Bind(KeyCode.T, "assign_waypoint");
            Console.Bind(KeyCode.J, "waypoint jump");
            Console.Bind(KeyCode.Z, "enemy_switch_animation_set");
            Console.Bind(KeyCode.X, "enemy_stop_all");
            Console.Bind(KeyCode.V, "set_enemy");
            Console.Bind(KeyCode.B, "enemy_set_destination");
            #endregion

            #region Buttons
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
                var enemies = FindObjectsOfType<EnemyInput>(true);
                foreach (var enemy in enemies)
                {
                    enemy.gameObject.Toggle();
                }
            });
            Console.AddButton("swap material", () =>
            {
                var things = FindObjectsOfType<MaterialSwap>(true);
                foreach (var thing in things)
                {
                    var rend = thing.GetComponent<MeshRenderer>();

                    var mat = rend.sharedMaterial;
                    if (mat.name == "Default")
                    {
                        rend.sharedMaterial = Resources.Load<Material>("DefaultUnlit");
                    }
                    else
                    {
                        rend.sharedMaterial = Resources.Load<Material>("Default");
                    }
                }
            });
            Console.AddButton("toggle cam", () =>
            {
                var cams = FindObjectsOfType<Camera>(true);
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
            #endregion

            /*
            #region Buttons
            Console.AddButton("Camera X -1", () => _enemyInput.SetCameraX(-1f));
            Console.AddButton("Camera X 0", () => _enemyInput.SetCameraX(0f));
            Console.AddButton("Camera X +1", () => _enemyInput.SetCameraX(1f));

            Console.AddButton("Horiz -1", () => _enemyInput.SetHorizontal(-1f));
            Console.AddButton("Horiz 0", () => _enemyInput.SetHorizontal(0f));
            Console.AddButton("Horiz +1", () => _enemyInput.SetHorizontal(1f));

            Console.AddButton("Vert -1", () => _enemyInput.SetVertical(-1f));
            Console.AddButton("Vert 0", () => _enemyInput.SetVertical(0f));
            Console.AddButton("Vert +1", () => _enemyInput.SetVertical(1f));

            Console.AddButton("Main cam", () => CameraTracker.Activate("main"));
            Console.AddButton("Enemy cam", () => CameraTracker.Activate("enemy"));

            Console.AddButton("Jump True", () => _enemyInput.SetJump(true));
            Console.AddButton("Jump False", () => _enemyInput.SetJump(false));

            Console.AddButton("0.1", () => Time.timeScale = 0.1f);
            Console.AddButton("0.2", () => Time.timeScale = 0.2f);
            Console.AddButton("0.5", () => Time.timeScale = 0.5f);
            Console.AddButton("0.75", () => Time.timeScale = 0.75f);

            Console.AddButton("Pause", () => Time.timeScale = 0f);
            Console.AddButton("Continue", () => Time.timeScale = 1f);
            Console.AddButton("1 Frame", () => Console.PlayOneFrame());

            Console.AddButton("Create waypoint", () => CreateWaypoint());
            Console.AddButton("Assign waypoint", () => AssignWaypoint());
            #endregion
            */
        }

        private T GetAtCrosshair<T>() where T : Component
        {
            var player = Tracker.Get<Character>("player");
            player.GetCrosshairInfo(out var hitInfo);

            var root = hitInfo.transform.GetComponent<T>();
            if (root) return root;

            return hitInfo.transform.GetComponentInChildren<T>();
        }

        private void CreateWaypoint(params string[] args)
        {
            var command = args.Length > 0 ? args[0] : "";

            var player = Tracker.Get<Character>("player");
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

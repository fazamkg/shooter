using UnityEngine;

namespace Faza
{
    public class EntryPoint : MonoBehaviour
    {
        private void Awake()
        {
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
                var name = GetAtCrosshair<EnemyAnimator>().IncrementAnimationSet();
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

            Console.Bind(KeyCode.Q, "waypoint");
            Console.Bind(KeyCode.R, "loop_waypoint");
            Console.Bind(KeyCode.T, "assign_waypoint");
            Console.Bind(KeyCode.J, "waypoint jump");
            Console.Bind(KeyCode.Z, "enemy_switch_animation_set");
            Console.Bind(KeyCode.X, "enemy_stop_all");
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

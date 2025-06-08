using UnityEngine;

namespace Faza
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private EnemyInput _enemyInput;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Camera _enemyCamera;

        private void Awake()
        {
            #region Commands
            Console.AddCommand("enemy_camera_x", (args) => _enemyInput.SetCameraX(args[0].ToFloat()));
            Console.AddCommand("enemy_horizontal", (args) => _enemyInput.SetHorizontal(args[0].ToFloat()));
            Console.AddCommand("enemy_vertical", (args) => _enemyInput.SetVertical(args[0].ToFloat()));
            Console.AddCommand("main_camera", (args) => CameraTracker.Activate("main"));
            Console.AddCommand("enemy_camera_fp", (args) => CameraTracker.Activate("enemy_fp"));
            Console.AddCommand("enemy_camera_tp", (args) => CameraTracker.Activate("enemy_tp"));
            Console.AddCommand("enemy_jump", (args) => _enemyInput.SetJump(args[0].ToBool()));
            Console.AddCommand("time_scale", (args) => Time.timeScale = args[0].ToFloat());
            Console.AddCommand("one_frame", (args) => Console.PlayOneFrame());
            Console.AddCommand("waypoint", (args) => CreateWaypoint());
            Console.AddCommand("loop_waypoint", (args) => Waypoint.LoopLastWaypoint());
            Console.AddCommand("assign_waypoint", (args) => AssignWaypoint());
            Console.AddCommand("show_waypoints", (args) => Waypoint.ShowAll());
            Console.AddCommand("hide_waypoints", (args) => Waypoint.HideAll());
            Console.AddCommand("enemy_acceleration", (args) =>
            {
                var enemy = Tracker.Get<Character>("enemy");
                enemy.SetAccelration(args[0].ToFloat());
            });
            Console.AddCommand("enemy_friction", (args) =>
            {
                var enemy = Tracker.Get<Character>("enemy");
                enemy.SetFriction(args[0].ToFloat());
            });

            Console.Bind(KeyCode.Q, "waypoint");
            Console.Bind(KeyCode.R, "loop_waypoint");
            Console.Bind(KeyCode.T, "assign_waypoint");
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

        private void CreateWaypoint()
        {
            var player = Tracker.Get<Character>("player");
            var hit = player.GetCrosshairInfo(out var hitInfo);
            if (hit)
            {
                var waypoint = Resources.Load<Waypoint>("Waypoint");
                var pos = hitInfo.point + hitInfo.normal * 0.5f;
                var instance = Instantiate(waypoint, pos, Quaternion.identity);
                instance.Init();
            }
        }

        private void AssignWaypoint()
        {
            var player = Tracker.Get<Character>("player");
            var hit = player.GetCrosshairInfo(out var hitInfo);
            if (hit)
            {
                var enemyInput = hitInfo.transform.GetComponent<EnemyInput>();
                if (enemyInput != null)
                {
                    var lastWaypoint = Waypoint.LastCreatedWaypoint;
                    var start = lastWaypoint.GetStart();
                    enemyInput.SetWaypoint(start);
                }
            }
        }
    } 
}

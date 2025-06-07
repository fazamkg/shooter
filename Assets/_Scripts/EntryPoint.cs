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
        }
    } 
}

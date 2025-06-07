using System.Collections;
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

            Console.AddButton("Jump True", () => _enemyInput.SetJump(true));
            Console.AddButton("Jump False", () => _enemyInput.SetJump(false));

            Console.AddButton("0.1", () => Time.timeScale = 0.1f);
            Console.AddButton("0.2", () => Time.timeScale = 0.2f);
            Console.AddButton("0.5", () => Time.timeScale = 0.5f);
            Console.AddButton("0.75", () => Time.timeScale = 0.75f);

            Console.AddButton("Pause", () => Time.timeScale = 0f);
            Console.AddButton("Continue", () => Time.timeScale = 1f);
            Console.AddButton("1 Frame", () =>
            {
                IEnumerator playOneFrame()
                {
                    Time.timeScale = 1f;
                    yield return null;
                    Time.timeScale = 0f;
                }
                Console.StartCoroutine_(playOneFrame());
            });
        }
    } 
}

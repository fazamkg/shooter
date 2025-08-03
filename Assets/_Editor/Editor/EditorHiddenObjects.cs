using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Faza
{
    public static class ToolsMenu
    {
        [MenuItem("Tools/Reveal Hidden GameObjects")]
        private static void RevealHiddenGameObjects()
        {
            var scene = SceneManager.GetActiveScene();
            foreach (var gameObject in scene.GetRootGameObjects())
            {
                RevealHiddenGameObject(gameObject);
            }
        }

        private static void RevealHiddenGameObject(GameObject gameObject)
        {
            if (gameObject.hideFlags.HasFlag(HideFlags.HideInHierarchy))
            {
                Debug.Log("Revealing hidden GameObject " + gameObject.name, gameObject);
                gameObject.hideFlags &= ~HideFlags.HideInHierarchy;
            }

            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                RevealHiddenGameObject(gameObject.transform.GetChild(i).gameObject);
            }
        }
    } 
}
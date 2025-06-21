using UnityEditor;
using UnityEngine;

namespace Faza
{
    [InitializeOnLoad]
    public class EditorWaypointConnector
    {
        private static GameObject _start;

        static EditorWaypointConnector()
        {
            SceneView.duringSceneGui += SceneView_duringSceneGui;
        }

        private static void SceneView_duringSceneGui(SceneView sceneView)
        {
            var e = Event.current;

            switch (e.type)
            {
                case EventType.MouseDown when e.button == 0 && e.shift:
                    _start = HandleUtility.PickGameObject(e.mousePosition, false);
                    if (_start != null) Debug.Log(_start.name);
                    e.Use();
                    break;

                case EventType.MouseUp when e.button == 0 && e.shift:
                    var end = HandleUtility.PickGameObject(e.mousePosition, false);
                    if (end != null) Debug.Log(end.name);
                    TryToConnect(end);
                    e.Use();
                    break;
            }
        }

        private static void TryToConnect(GameObject end)
        {
            if (_start == null) return;
            if (end == null) return;

            var startWp = _start.GetComponent<Waypoint>();
            if (startWp == null) return;

            var endWp = end.GetComponent<Waypoint>();
            if (endWp == null) return;

            startWp.AddConnection(endWp);
            endWp.AddConnection(startWp);

            EditorUtility.SetDirty(startWp);
            EditorUtility.SetDirty(endWp);
        }
    } 
}

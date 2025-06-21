using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    [DefaultExecutionOrder(-100)]
    public class Line : MonoBehaviour
    {
        [SerializeField] private LineRenderer _prefab;

        private static Vector3[] _positions = new Vector3[2];

        private static Stack<LineRenderer> _activeLines = new();

        private void Awake()
        {
            Pool.SetPrefab("line", _prefab);
        }

        // super early update
        private void Update()
        {
            while (_activeLines.Count != 0)
            {
                var line = _activeLines.Pop();
                Pool.Release("line", line);
            }
        }

        public static void Draw(Vector3 from, Vector3 to, Color? color = null)
        {
            var line = Pool.Get<LineRenderer>("line");
            _positions[0] = from;
            _positions[1] = to;
            line.SetPositions(_positions);
            line.startColor = color ?? Color.red;
            line.endColor = color ?? Color.red;
            _activeLines.Push(line);
        }
    } 
}

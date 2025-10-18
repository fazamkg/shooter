using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    [DefaultExecutionOrder(-100)]
    public class Line : MonoBehaviour
    {
        private const string POOL_KEY = "line";

        [SerializeField] private LineRenderer _prefab;

        private static Vector3[] _positions = new Vector3[2];

        private static Stack<LineRenderer> _activeLines = new();
        private static Stack<LineRenderer> _permaLines = new();

        private void Awake()
        {
            Pool.SetPrefab(POOL_KEY, _prefab);
        }

        private void Update()
        {
            while (_activeLines.Count != 0)
            {
                var line = _activeLines.Pop();
                Pool.Release(POOL_KEY, line);
            }
        }

        public static void ClearLines()
        {
            while (_permaLines.Count != 0)
            {
                var line = _permaLines.Pop();
                Pool.Release(POOL_KEY, line);
            }
        }

        public static void Draw(Vector3 from, Vector3 to, Color? color = null, bool permanent = false)
        {
            var line = Pool.Get<LineRenderer>(POOL_KEY);
            _positions[0] = from;
            _positions[1] = to;
            line.SetPositions(_positions);
            line.startColor = color ?? Color.red;
            line.endColor = color ?? Color.red;
            if (permanent == false)
            {
                _activeLines.Push(line);
            }
            else
            {
                _permaLines.Push(line);
            }
        }

        public static void DrawRay(Vector3 from, Vector3 direction, Color? color = null, bool permanent = false)
        {
            Draw(from, from + direction, color, permanent);
        }

        public static void DrawPermanent(Vector3 from, Vector3 to, Color? color = null)
        {
            Draw(from, to, color, true);
        }

        public static void DrawRayPermanent(Vector3 from, Vector3 direction, Color? color = null)
        {
            DrawRay(from, direction, color, true);
        }
    }
}

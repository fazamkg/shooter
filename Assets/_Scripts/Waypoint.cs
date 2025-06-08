using UnityEngine;

namespace Faza
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        private static Waypoint _firstCreatedWaypoint;
        private static Waypoint _lastCreatedWaypoint;

        private Waypoint _next;
        private Waypoint _prev;

        public static Waypoint LastCreatedWaypoint => _lastCreatedWaypoint;
        public Waypoint Next => _next;

        public void Init()
        {
            if (_firstCreatedWaypoint == null)
            {
                _firstCreatedWaypoint = this;
            }

            if (_lastCreatedWaypoint != null)
            {
                _lastCreatedWaypoint._next = this;
                _prev = _lastCreatedWaypoint;

                var renderer = _lastCreatedWaypoint._lineRenderer;

                var positions = new Vector3[] { renderer.transform.position, transform.position };
                renderer.SetPositions(positions);
            }

            _lastCreatedWaypoint = this;
        }

        public Waypoint GetStart()
        {
            return _firstCreatedWaypoint;

            var start = this;

            while (start._prev != null)
            {
                start = start._prev;
            }

            return start;
        }

        public Waypoint GetEnd()
        {
            var end = this;

            while (end._next != null)
            {
                end = end._next;
            }

            return end;
        }

        public static void LoopLastWaypoint()
        {
            var start = _lastCreatedWaypoint.GetStart();

            _lastCreatedWaypoint._next = start;
            start._prev = _lastCreatedWaypoint;

            var renderer = _lastCreatedWaypoint._lineRenderer;
            var positions = new Vector3[] { renderer.transform.position, start.transform.position };
            renderer.SetPositions(positions);
        }
    }
}

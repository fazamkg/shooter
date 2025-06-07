using UnityEngine;

namespace Faza
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        private static Waypoint _lastCreatedWaypoint;

        private Waypoint _next;
        private Waypoint _prev;

        public static Waypoint LastCreatedWaypoint => _lastCreatedWaypoint;

        public void Init()
        {
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
    }
}

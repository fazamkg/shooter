using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Faza
{
    public class Waypoint : MonoBehaviour, IWaypoint
    {
        [SerializeField] private string _command = "";
        [SerializeField] private List<Waypoint> _connections;
        [SerializeField] private MeshRenderer _meshRenderer;

        private static List<Waypoint> _all = new();

        private static Waypoint _firstCreatedWaypoint;
        private static Waypoint _lastCreatedWaypoint;

        private Waypoint _next;
        private Waypoint _prev;

        private Waypoint _parent;
        private float _g;
        private float _h;
        private float _f = float.MaxValue;
        private bool _isMarked;

        public static Waypoint LastCreatedWaypoint => _lastCreatedWaypoint;
        public Waypoint Next => _next;
        public string Command => _command;
        public Vector3 Pos => transform.position;
        public static List<Waypoint> All => _all;
        public bool IsMarked
        {
            get => _isMarked;
            set
            {
                _isMarked = value;

                if (_isMarked)
                {
                    var mpb = new MaterialPropertyBlock();
                    mpb.SetColor(ShaderKey.BaseColor, Color.yellow);
                    _meshRenderer.SetPropertyBlock(mpb);
                }
                else
                {
                    _meshRenderer.SetPropertyBlock(null);
                }
            }
        }

        public void Init(string command)
        {
            if (_firstCreatedWaypoint == null)
            {
                _firstCreatedWaypoint = this;
            }

            if (_lastCreatedWaypoint != null)
            {
                _lastCreatedWaypoint._next = this;
                _prev = _lastCreatedWaypoint;
            }

            _lastCreatedWaypoint = this;

            _command = command;
        }

        private void Awake()
        {
            _all.Add(this);

            Hide();
        }

        private void Update()
        {
            if (_meshRenderer.enabled == false) return;

            foreach (var connection in _connections)
            {
                var color = IsMarked && connection.IsMarked ? Color.yellow : Color.red;
                Line.Draw(Pos, connection.Pos, color);
            }
        }

        public Waypoint GetStart()
        {
            return _firstCreatedWaypoint;
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
        }

        public void Show()
        {
            _meshRenderer.enabled = true;
        }

        public void Hide()
        {
            _meshRenderer.enabled = false;
        }

        public static void HideAll()
        {
            foreach (var wp in _all)
            {
                wp.Hide();
            }
        }

        public static void ShowAll()
        {
            foreach (var wp in _all)
            {
                wp.Show();
            }
        }

        public static Waypoint Closest(Vector3 to)
        {
            var result = _all[0];

            var order = _all.OrderByDescending(x => (to - x.Pos).sqrMagnitude);
            var orderedWaypoints = new Stack<Waypoint>(order);

            while (orderedWaypoints.Count != 0)
            {
                result = orderedWaypoints.Pop();
                var direction = (result.Pos - to).normalized;

                var ray = new Ray(to, direction);
                var hit = Physics.Raycast(ray, out var hitInfo, 1000f);
                if (hit)
                {
                    var isWp = hitInfo.collider.GetComponent<Waypoint>();
                    if (isWp) break;
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        public List<Waypoint> FindPath(Waypoint target)
        {
            foreach (var wp in _all)
            {
                wp._f = float.MaxValue;
                wp._h = 0f;
                wp._g = 0f;
                wp._parent = null;
            }

            var open = new HashSet<Waypoint>();
            var closed = new HashSet<Waypoint>();
            open.Add(this);

            while (open.Count != 0)
            {
                var current = open.OrderBy(x => x._f).First();
                open.Remove(current);
                closed.Add(current);

                if (current == target)
                {
                    var path = new List<Waypoint>();
                    var n = target;
                    while (n != null)
                    {
                        path.Add(n);
                        n = n._parent;
                    }
                    path.Reverse();
                    return path;
                }

                foreach (var neighbour in current._connections)
                {
                    if (closed.Contains(neighbour))
                    {
                        continue;
                    }

                    var g = current._g + Vector3.Distance(current.Pos, neighbour.Pos);
                    var h = Vector3.Distance(neighbour.Pos, target.Pos);
                    var f = g + h;

                    if (f < neighbour._f || open.Contains(neighbour) == false)
                    {
                        neighbour._g = g;
                        neighbour._h = h;
                        neighbour._f = f;
                        neighbour._parent = current;

                        open.Add(neighbour);
                    }
                }
            }

            return null;
        }

        public void AddConnection(Waypoint waypoint)
        {
            if (_connections.Contains(waypoint)) return;

            _connections.Add(waypoint);
        }

        public void RemoveConnection(Waypoint waypoint)
        {
            if (_connections.Contains(waypoint) == false) return;

            _connections.Remove(waypoint);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var connection in _connections)
            {
                Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
                Handles.color = Color.red;
                Handles.DrawLine(transform.position, connection.transform.position, 2f);
            }
        }
#endif
    }
}

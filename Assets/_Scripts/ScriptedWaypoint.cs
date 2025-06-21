using UnityEngine;

namespace Faza
{
    public class ScriptedWaypoint : IWaypoint
    {
        private string _command;
        private Vector3 _position;

        public bool IsMarked { get; set; }
        public Vector3 Pos => _position;
        public string Command => _command;

        public ScriptedWaypoint(Vector3 position, string command = "")
        {
            _command = command;
            _position = position;
        }
    }
}

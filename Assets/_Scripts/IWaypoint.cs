using UnityEngine;

namespace Faza
{
    public interface IWaypoint
    {
        public bool IsMarked { get; set; }

        public Vector3 Pos { get; }

        public string Command { get; }
    } 
}

using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    public static class Joystick
    {
        private static Dictionary<string, Vector3> _inputs = new();

        public static void SetInput(string name, Vector3 input)
        {
            _inputs[name] = input;
        }

        public static Vector3 GetInput(string name)
        {
            _inputs.TryGetValue(name, out var value);
            return value;
        }
    } 
}

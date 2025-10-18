using UnityEngine;

namespace Faza
{
    public static class ShaderKey
    {
        public static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        public static readonly int Position = Shader.PropertyToID("_Position");
        public static readonly int Radius = Shader.PropertyToID("_Radius");
        public static readonly int Color = Shader.PropertyToID("_Color");
    }
}

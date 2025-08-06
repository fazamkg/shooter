using System;

namespace Faza
{
    [Serializable]
    public abstract class Effect
    {
        public abstract string DisplayValue { get; }
        public abstract void Apply();
    }
}

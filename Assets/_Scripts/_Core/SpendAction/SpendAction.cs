using System;
using UnityEngine;

namespace Faza
{
    [Serializable]
    public abstract class SpendAction
    {
        public abstract Sprite Sprite { get; }

        public abstract string Cost { get; }

        public abstract bool CanSpend();

        public abstract void Spend(Action onSucess, Action onFailure);
    } 
}

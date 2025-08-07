using System;

namespace Faza
{
    [Serializable]
    public abstract class SpendAction
    {
        public abstract bool CanSpend();

        public abstract void Spend(Action onSucess, Action onFailure);
    } 
}

using System;

namespace Faza
{
    [Serializable]
    public class RvSpendAction : SpendAction
    {
        public override bool CanSpend()
        {
            return true;
        }

        public override void Spend(Action onSucess, Action onFailure)
        {
            onSucess?.Invoke();
        }
    }
}

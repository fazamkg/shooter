using System;
using UnityEngine;

namespace Faza
{
    [Serializable]
    public class RvSpendAction : SpendAction
    {
        public override Sprite Sprite => Resources.Load<Sprite>("Rv");

        public override string Cost => "БЕСПЛ.";

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

using UnityEngine;
using System;

namespace Faza
{
    [Serializable]
    public class ModifierEffect : Effect
    {
        [SerializeField] private string _modifierId;
        [SerializeField] private ModifierType _type;
        [SerializeField] private Modifier _toApply;

        public override string DisplayValue => "";

        public override void Apply()
        {
            var mod = Modifier.Get(_modifierId);
            mod.AddModifier(_type, _toApply);
        }

        public override void Remove()
        {
            var mod = Modifier.Get(_modifierId);
            mod.RemoveModifier(_type, _toApply);
        }
    }
}

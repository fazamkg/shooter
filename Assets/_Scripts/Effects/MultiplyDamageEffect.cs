using UnityEngine;
using System;

namespace Faza
{
    [Serializable]
    public class MultiplyDamageEffect : Effect
    {
        [SerializeField] private float _mult;
        [SerializeField] private float _duration;

        public override string DisplayValue => _mult.ToString();


        public override void Apply()
        {
            // add multiplier for a duration of ingame time

            var player = PlayerInput.Instance;
            player.Shooter.AddDamage(0f);

            //player.Shooter.TheDamage.AddModifier(ModifierType.Mult, _mult);
        }

        public override void Remove()
        {
        }
    }
}

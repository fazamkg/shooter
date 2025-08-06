using UnityEngine;
using System;

namespace Faza
{
    [Serializable]
    public class AddDamageEffect : Effect
    {
        [SerializeField] private float _damage;

        public override string DisplayValue =>  _damage.ToString();

        public override void Apply()
        {
            var player = PlayerInput.Instance;
            player.Shooter.AddDamage(_damage);
        }
    }
}

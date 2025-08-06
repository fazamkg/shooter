using UnityEngine;
using System;

namespace Faza
{
    [Serializable]
    public class AddShootingSpeedEffect : Effect
    {
        [SerializeField] private float _shootingSpeed;

        public override string DisplayValue => _shootingSpeed.ToString();

        public override void Apply()
        {
            var player = PlayerInput.Instance;
            player.Shooter.AddShootingSpeed(_shootingSpeed);
        }
    }
}

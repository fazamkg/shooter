using UnityEngine;
using System;

namespace Faza
{
    [Serializable]
    public class AddProjectileSpeedEffect : Effect
    {
        [SerializeField] private float _projectileSpeed;

        public override string DisplayValue => _projectileSpeed.ToString();

        public override void Apply()
        {
            var player = PlayerInput.Instance;
            player.Shooter.AddProjectileSpeed(_projectileSpeed);
        }
    }
}

using UnityEngine;
using System;

namespace Faza
{
    [Serializable]
    public class AddMoveSpeedEffect : Effect
    {
        [SerializeField] private float _speed;

        public override string DisplayValue => _speed.ToString();

        public override void Apply()
        {
            var player = PlayerInput.Instance;
            player.Character.AddSpeed(_speed);
        }
    }
}

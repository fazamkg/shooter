using System;

namespace Faza
{
    [Serializable]
    public class AddGodEffect : Effect
    {
        public override string DisplayValue => "";

        public override void Apply()
        {
            var player = PlayerInput.Instance;
            player.Health.God = true;
        }

        public override void Remove()
        {
            var player = PlayerInput.Instance;
            player.Health.God = false;
        }
    }
}
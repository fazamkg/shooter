using System;

namespace Faza
{
    [Serializable]
    public class AddCritGlowEffect : Effect
    {
        public override string DisplayValue => "";

        public override void Apply()
        {
            var player = PlayerInput.Instance;
            player.ActivateCritGlow();
        }

        public override void Remove()
        {
            var player = PlayerInput.Instance;
            player.DeactivateCritGlow();
        }
    }
}
using System;

namespace Faza
{
    [Serializable]
    public class AddArmorGlowEffect : Effect
    {
        public override string DisplayValue => "";

        public override void Apply()
        {
            var player = PlayerInput.Instance;
            player.ActivateArmorGlow();
        }

        public override void Remove()
        {
            var player = PlayerInput.Instance;
            player.DeactivateArmorGlow();
        }
    }
}

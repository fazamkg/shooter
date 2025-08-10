using System;

namespace Faza
{
    [Serializable]
    public class AddHasteGlowEffect : Effect
    {
        public override string DisplayValue => "";

        public override void Apply()
        {
            var player = PlayerInput.Instance;
            player.ActivateHasteGlow();
        }

        public override void Remove()
        {
            var player = PlayerInput.Instance;
            player.DeactivateHasteGlow();
        }
    }
}
using UnityEngine;

namespace Faza
{
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private UpgradeGroupData[] _availableUpgrades;
        [SerializeField] private BoosterData[] _boostersToUnlock;

        public UpgradeGroupData[] AvailableUpgrades => _availableUpgrades;

        public void UnlockBoosters()
        {
            if (_boostersToUnlock != null && _boostersToUnlock.Length > 0)
            {
                foreach (var booster in _boostersToUnlock)
                {
                    if (booster.IsUnlocked == false)
                    {
                        booster.IsUnlocked = true;
                        booster.GiveFreeAmount();
                    }
                }
            }
        }
    } 
}

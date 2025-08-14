using System;
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

        public void SetCompletedTimespan(TimeSpan elapsed)
        {
            var stored = Storage.GetTimeSpan($"faza_{name}_timespan", TimeSpan.MaxValue);
            if (elapsed < stored)
            {
                Storage.SetTimeSpan($"faza_{name}_timespan", elapsed);
            }
        }

        public TimeSpan GetCompletedTimespan()
        {
            return Storage.GetTimeSpan($"faza_{name}_timespan", TimeSpan.MaxValue);
        }
    } 
}

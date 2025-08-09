using UnityEngine;

namespace Faza
{
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private UpgradeGroupData[] _availableUpgrades;
        [SerializeField] private BoosterData[] _availableBoosters;

        public UpgradeGroupData[] AvailableUpgrades => _availableUpgrades;
        public BoosterData[] AvailableBoosters => _availableBoosters;
    } 
}

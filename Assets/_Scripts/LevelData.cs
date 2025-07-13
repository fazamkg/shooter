using UnityEngine;

namespace Faza
{
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private UpgradeGroupData[] _availableUpgrades;


        public UpgradeGroupData[] AvailableUpgrades => _availableUpgrades;
    } 
}

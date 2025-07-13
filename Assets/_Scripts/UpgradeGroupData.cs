using UnityEngine;

namespace Faza
{
    [CreateAssetMenu]
    public class UpgradeGroupData : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private UpgradeData[] _chain;

        public string Id => _id;
        public UpgradeData[] Chain => _chain;
    } 
}

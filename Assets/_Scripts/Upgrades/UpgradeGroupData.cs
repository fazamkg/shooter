using System.Collections.Generic;
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

        public List<UpgradeData> GetPurchasedUpgrades()
        {
            var list = new List<UpgradeData>();
            var index = Storage.GetInt(_id);

            for (var i = 0; i < index; i++)
            {
                list.Add(_chain[i]);
            }

            return list;
        }

        public UpgradeData GetCurrentToPurhase()
        {
            var index = Storage.GetInt(_id);
            return _chain[index];
        }

        public bool Purchase()
        {
            var index = Storage.GetInt(_id);
            var current = _chain[index];

            if (current.IsMaxed) return false;
            if (Currency.Coins < current.Cost) return false;

            FazaAudio.Play("upgrade");

            current.Apply();

            Currency.RemoveCoins(current.Cost);

            Storage.SetInt(_id, index + 1);

            return true;
        }
    } 
}

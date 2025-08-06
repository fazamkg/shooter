using System.Linq;
using UnityEngine;

namespace Faza
{
    [CreateAssetMenu]
    public class UpgradeData : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private bool _maxed;
        [TextArea]
        [SerializeField] private string _description;
        [SerializeField] private float _cost;
        [SerializeReference, SubclassSelector] private Effect[] _effects;

        public Sprite Icon => _icon;
        public string Name => _name;
        public bool IsMaxed => _maxed;
        public string Description => string.Format(_description, _effects.Select(x => x.DisplayValue).ToArray());
        public float Cost => _cost;

        public void Apply()
        {
            foreach (var effect in _effects)
            {
                effect.Apply();
            }
        }
    } 
}

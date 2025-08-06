using UnityEngine;

namespace Faza
{
    [CreateAssetMenu]
    public class BoosterData : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _duration;
    } 
}

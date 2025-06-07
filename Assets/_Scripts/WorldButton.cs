using UnityEngine;
using UnityEngine.Events;

namespace Faza
{
    public class WorldButton : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onUse;

        public void Use()
        {
            _onUse?.Invoke();
        }
    } 
}

using UnityEngine;

namespace Faza
{
    public class CoinTrigger : MonoBehaviour
    {
        [SerializeField] private Coin _coin;

        private void OnTriggerEnter(Collider other)
        {
            _coin.MyOnTriggerEnter(other);
        }
    } 
}

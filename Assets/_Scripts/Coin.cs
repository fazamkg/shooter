using UnityEngine;

namespace Faza
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private void Update()
        {
            transform.Rotate(0f, Time.deltaTime * _speed, 0f, Space.World);
        }
    } 
}

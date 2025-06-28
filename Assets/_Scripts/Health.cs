using UnityEngine;
using System;

namespace Faza
{
    public class Health : MonoBehaviour
    {
        public event Action OnDeath;

        [SerializeField] private float _startHealth;

        public float CurrentHealth { get; private set; }

        private void Awake()
        {
            CurrentHealth = _startHealth;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0f)
            {
                CurrentHealth = 0f;
                OnDeath?.Invoke();
            }
        }
    } 
}

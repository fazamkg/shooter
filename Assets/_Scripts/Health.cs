using UnityEngine;
using System;

namespace Faza
{
    public class Health : MonoBehaviour
    {
        public static event Action<Health> OnHealthCreated;

        public event Action OnDeath;
        public event Action OnHealthChanged;

        [SerializeField] private float _startHealth;
        [SerializeField] private float _maxHealth;
        [SerializeField] private Transform _healthbarPoint;

        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }
        public Transform HealthbarPoint => _healthbarPoint;

        private void Awake()
        {
            CurrentHealth = _startHealth;
            MaxHealth = _maxHealth;
            OnHealthCreated?.Invoke(this);
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0f)
            {
                CurrentHealth = 0f;
                OnDeath?.Invoke();
            }

            OnHealthChanged?.Invoke();
        }
    } 
}

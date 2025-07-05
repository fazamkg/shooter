using UnityEngine;
using System;

namespace Faza
{
    public class Health : MonoBehaviour
    {
        public static event Action<Health> OnHealthCreated;
        public static event Action<Health> OnHealthDestroyed;

        public event Action OnDeath;
        public event Action OnHealthChanged;

        [SerializeField] private float _startHealth;
        [SerializeField] private float _maxHealth;
        [SerializeField] private bool _allowViewCreation = true;
        [SerializeField] private Character _character;

        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }
        public Transform HealthbarPoint => GetComponentInChildren<CharacterAnimator>().HealthbarPoint;
        public bool AllowViewCreation => _allowViewCreation;
        public bool IsDead => CurrentHealth <= 0f;

        private void Awake()
        {
            CurrentHealth = _startHealth;
            MaxHealth = _maxHealth;
            OnHealthCreated?.Invoke(this);
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;

            if (_character != null)
            {
                _character.Stop(0.5f);
            }

            if (CurrentHealth <= 0f)
            {
                CurrentHealth = 0f;

                // todo: do not use getcomponent
                GetComponent<CharacterController>().enabled = false;

                OnDeath?.Invoke();
                OnHealthDestroyed?.Invoke(this);
            }

            OnHealthChanged?.Invoke();
        }
    } 
}

using UnityEngine;
using System;

namespace Faza
{
    public class Health : MonoBehaviour
    {
        public static event Action OnDeathGlobal;

        public static event Action<Health> OnHealthCreated;
        public static event Action<Health> OnHealthDestroyed;
        public static event Action<Health> OnTakenDamage;

        public event Action OnDeath;
        public event Action OnHealthChanged;

        [SerializeField] private float _startHealth;
        [SerializeField] private float _maxHealth;
        [SerializeField] private bool _allowViewCreation = true;
        [SerializeField] private Character _character;
        [SerializeField] private float _stunDuration = 0.5f;

        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }
        public Transform HealthbarPoint => GetComponentInChildren<CharacterAnimator>().HealthbarPoint;
        public bool AllowViewCreation => _allowViewCreation;
        public bool IsDead => CurrentHealth <= 0f;
        public Vector3 LastDamageDirection { get; private set; }

        private void Awake()
        {
            CurrentHealth = _startHealth;
            MaxHealth = _maxHealth;
            OnHealthCreated?.Invoke(this);
        }

        public void TakeDamage(float damage, Vector3 damageDirection, bool stopCharacter = true)
        {
            LastDamageDirection = damageDirection;

            CurrentHealth -= damage;

            OnTakenDamage?.Invoke(this);

            if (_character != null && stopCharacter)
            {
                _character.Stop(_stunDuration);
            }

            if (CurrentHealth <= 0f)
            {
                CurrentHealth = 0f;

                // todo: do not use getcomponent
                GetComponent<CharacterController>().enabled = false;

                OnDeath?.Invoke();
                OnDeathGlobal?.Invoke();
                OnHealthDestroyed?.Invoke(this);
            }

            OnHealthChanged?.Invoke();
        }
    } 
}

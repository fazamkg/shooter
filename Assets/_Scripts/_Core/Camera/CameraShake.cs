using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class CameraShake : MonoBehaviour
    {
        [Header("Player Hit")]
        [SerializeField] private float _duration;
        [SerializeField] private float _strength;
        [SerializeField] private int _vibrato;

        [Header("Enemy Hit")]
        [SerializeField] private float _enemyDuration;
        [SerializeField] private float _enemyStrength;
        [SerializeField] private int _enemyVibrato;

        private void Awake()
        {
            Health.OnTakenDamage += Health_OnTakenDamage;
        }

        private void OnDestroy()
        {
            Health.OnTakenDamage -= Health_OnTakenDamage;
        }

        private void Health_OnTakenDamage(Health health)
        {
            if (health.GetComponent<PlayerInput>())
            {
                ShakePlayer();
            }
            else
            {
                ShakeEnemy();
            }
        }

        private void ShakePlayer()
        {
            transform.DOKill();
            transform.DOShakePosition(_duration, _strength, _vibrato);
        }

        private void ShakeEnemy()
        {
            transform.DOKill();
            transform.DOShakePosition(_enemyDuration, _enemyStrength, _enemyVibrato);
        }
    } 
}

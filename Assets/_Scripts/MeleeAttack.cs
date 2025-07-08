using System.Collections;
using UnityEngine;

namespace Faza
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private float _range;
        [SerializeField] private float _damage;
        [SerializeField] private Health _health;
        [SerializeField] private float _finishDelay;

        private Collider[] _colliders = new Collider[32];

        public bool WithinAttack { get; private set; }
        public bool StartAttack { get; set; }
        public float Damage => _damage;
        public Transform Target { get; set; }
        public float Range => _range;

        private void Awake()
        {
            _health.OnDeath += Health_OnDeath;
        }

        private void Health_OnDeath()
        {
            enabled = false;
        }

        private void Update()
        {
            if (WithinAttack) return;
            
            var amount = Physics.OverlapSphereNonAlloc(transform.position, _range, _colliders);
            for (var i = 0; i < amount; i++)
            {
                var collider = _colliders[i];
                var playerInput = collider.GetComponent<PlayerInput>();

                if (playerInput)
                {
                    Target = playerInput.transform;
                    StartAttack = true;
                    WithinAttack = true;
                }
            }
        }

        private IEnumerator FinishAttackCoroutine()
        {
            yield return new WaitForSeconds(_finishDelay);
            WithinAttack = false;
        }

        public void FinishAttack()
        {
            StartCoroutine(FinishAttackCoroutine());
        }
    } 
}

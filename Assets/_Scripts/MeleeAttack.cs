using System.Collections;
using UnityEngine;

namespace Faza
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private float _range;
        [SerializeField] private float _damage;

        private Collider[] _colliders = new Collider[32];

        public bool WithinAttack { get; private set; }
        public bool StartAttack { get; set; }
        public float Damage => _damage;
        public Transform Target { get; set; }

        private void Update()
        {
            if (WithinAttack) return;
            if (_character.HorizontalSpeed > 1f) return;
            
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

        public void FinishAttack()
        {
            WithinAttack = false;
        }
    } 
}

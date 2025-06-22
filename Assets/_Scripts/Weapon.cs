using UnityEngine;

namespace Faza
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private CharacterInput _input;
        [SerializeField] private AbstractAnimation _weaponAnim;
        [SerializeField] private float _shootCooldown;

        private float _cooldown;

        private void Update()
        {
            _cooldown -= Time.deltaTime;
            if (_cooldown < 0f)
            {
                _cooldown = 0f;
            }

            if (_cooldown > 0f)
            {
                return;
            }

            if (_input.IsFire())
            {
                _cooldown = _shootCooldown;

                _weaponAnim.Play();
            }
        }
    } 
}

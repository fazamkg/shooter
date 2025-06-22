using UnityEngine;

namespace Faza
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private CharacterInput _input;
        [SerializeField] private AbstractAnimation _weaponAnim;
        [SerializeField] private float _shootCooldown;
        [SerializeField] private Transform _origin;
        [SerializeField] private GameObject _bulletHolePrefab;

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

                var ray = new Ray(_origin.position, _origin.forward);
                var hit = Physics.Raycast(ray, out var info);
                if (hit)
                {
                    var pos = info.point + info.normal * 0.1f;
                    var rot = Quaternion.LookRotation(-info.normal);
                    Instantiate(_bulletHolePrefab, pos, rot, info.collider.transform);
                }
            }
        }
    } 
}

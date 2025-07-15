using UnityEngine;

namespace Faza
{
    [CreateAssetMenu]
    public class UpgradeData : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private bool _maxed;
        [TextArea]
        [SerializeField] private string _description;
        [SerializeField] private float _cost;
        [SerializeField] private float _damage;
        [SerializeField] private float _speed;
        [SerializeField] private float _shootingSpeed;
        [SerializeField] private float _projectileSpeed;

        public Sprite Icon => _icon;
        public string Name => _name;
        public bool IsMaxed => _maxed;
        public string Description => string.Format
            (_description, _damage, _speed, _shootingSpeed, _projectileSpeed);
        public float Cost => _cost;
        public float Damage => _damage;
        public float Speed => _speed;
        public float ShootingSpeed => _shootingSpeed;
        public float ProjectileSpeed => _projectileSpeed;

        public void Apply()
        {
            var player = PlayerInput.Instance;
            player.Shooter.AddDamage(_damage);
            player.Character.AddSpeed(_speed);
            player.Shooter.AddProjectileSpeed(_projectileSpeed);
            player.Shooter.AddShootingSpeed(_shootingSpeed);
        }
    } 
}

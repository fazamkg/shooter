using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    // todo: remove all execution orders and use explicit order in code instead
    [DefaultExecutionOrder(-1)]
    public class ViewManager : MonoBehaviour
    {
        [SerializeField] private HealthView _healthViewPrefab;
        [SerializeField] private Transform _healthViewParent;
        [SerializeField] private HealthView _playerHealthView;
        [SerializeField] private Health _playerHealth;
        [SerializeField] private SkullsView _skullsView;

        private Dictionary<Health, HealthView> _healths = new();

        private void Awake()
        {
            Health.OnHealthCreated += Health_OnHealthCreated;
            Health.OnHealthDestroyed += Health_OnHealthDestroyed;
        }

        private void Start()
        {
            _playerHealthView.Init(_playerHealth);
            _skullsView.Init();
        }

        private void OnDestroy()
        {
            Health.OnHealthCreated -= Health_OnHealthCreated;
            Health.OnHealthDestroyed -= Health_OnHealthDestroyed;
        }

        private void Health_OnHealthCreated(Health health)
        {
            if (health.AllowViewCreation == false) return;

            var view = Instantiate(_healthViewPrefab, _healthViewParent);
            view.Init(health);

            _healths[health] = view;
        }

        private void Health_OnHealthDestroyed(Health health)
        {
            if (health.AllowViewCreation == false) return;

            _healths[health].TweenDisappear();
            _healths.Remove(health);
        }
    } 
}

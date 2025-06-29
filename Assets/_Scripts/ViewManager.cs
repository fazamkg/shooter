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

        private Dictionary<Health, HealthView> _healths = new();

        private void Awake()
        {
            Health.OnHealthCreated += Health_OnHealthCreated;
            Health.OnHealthDestroyed += Health_OnHealthDestroyed;
        }

        private void Health_OnHealthCreated(Health health)
        {
            var view = Instantiate(_healthViewPrefab, _healthViewParent);
            view.Init(health);

            _healths[health] = view;
        }

        private void Health_OnHealthDestroyed(Health health)
        {
            _healths[health].TweenDisappear();
            _healths.Remove(health);
        }
    } 
}

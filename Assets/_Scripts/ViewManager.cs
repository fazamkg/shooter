using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    [DefaultExecutionOrder(-1)]
    public class ViewManager : MonoBehaviour
    {
        [SerializeField] private HealthView _healthViewPrefab;
        [SerializeField] private Transform _healthViewParent;

        private Dictionary<Health, HealthView> _healths = new();

        private void Awake()
        {
            Health.OnHealthCreated += Health_OnHealthCreated;
        }

        private void Health_OnHealthCreated(Health health)
        {
            var view = Instantiate(_healthViewPrefab, _healthViewParent);
            view.Init(health);

            _healths[health] = view;
        }
    } 
}

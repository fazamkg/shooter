using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    public class SkullsView : MonoBehaviour
    {
        [SerializeField] private SkullView _skullViewPrefab;

        private List<SkullView> _views = new();

        public void Init()
        {
            foreach (var enemy in EnemyInput.AllEnemies)
            {
                var view = Instantiate(_skullViewPrefab, transform);
                _views.Add(view);
            }

            Health.OnHealthDestroyed += Health_OnHealthDestroyed;
        }

        private void OnDestroy()
        {
            Health.OnHealthDestroyed -= Health_OnHealthDestroyed;
        }

        private void Health_OnHealthDestroyed(Health health)
        {
            if (health.GetComponent<PlayerInput>()) return;

            _views.Find(x => x.CrossedOut == false).CrossOut();
        }
    } 
}

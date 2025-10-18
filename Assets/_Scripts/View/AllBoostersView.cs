using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    public class AllBoostersView : MonoBehaviour
    {
        [SerializeField] private BoosterView _boosterViewPrefab;
        [SerializeField] private Transform _boosterParent;

        public void Init(List<BoosterData> availableBoosters)
        {
            foreach (var booster in availableBoosters)
            {
                var view = Instantiate(_boosterViewPrefab, _boosterParent);
                view.Init(booster);
            }
        }
    } 
}

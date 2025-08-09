using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Faza
{
    [CreateAssetMenu]
    public class BoosterData : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _purchaseCount;
        [SerializeField] private int _boosterAmount;
        [SerializeField] private int _altPurchaseCount;
        [SerializeField] private int _altBoosterAmount;
        [SerializeReference, SubclassSelector] private SpendAction _spendAction;
        [SerializeReference, SubclassSelector] private SpendAction _altSpendAction;
        [SerializeField] private float _duration;
        [SerializeReference, SubclassSelector] private Effect[] _effects;

        private static HashSet<BoosterData> _runningBoosters = new();

        public Sprite Icon => _icon;

        public int AmountPref
        {
            get => Storage.GetInt($"{_id}_amount");
            private set => Storage.SetInt($"{_id}_amount", value);
        }

        public int PurchaseCountPref
        {
            get => Storage.GetInt($"{_id}_purchases");
            private set => Storage.SetInt($"{_id}_purchases", value);
        }

        public int AltPurchaseCountPref
        {
            get => Storage.GetInt($"{_id}_alt_purchases");
            private set => Storage.SetInt($"{_id}_alt_purchases", value);
        }

        private void OnSuccess()
        {
            PurchaseCountPref++;
            if (PurchaseCountPref >= _purchaseCount)
            {
                PurchaseCountPref = 0;
                AmountPref += _boosterAmount;
            }
        }

        private void OnSuccessAlt()
        {
            AltPurchaseCountPref++;
            if (AltPurchaseCountPref >= _altPurchaseCount)
            {
                AltPurchaseCountPref = 0;
                AmountPref += _altBoosterAmount;
            }
        }

        private IEnumerator WaitBoosterCoroutine()
        {
            yield return new WaitForSeconds(_duration);
            foreach (var effect in _effects)
            {
                effect.Remove();
            }

            _runningBoosters.Remove(this);
        }

        public void Apply()
        {
            if (_runningBoosters.Contains(this)) return;

            if (AmountPref <= 0) return;

            foreach (var effect in _effects)
            {
                effect.Apply();
            }

            _runningBoosters.Add(this);

            Routines.StartCoroutine_(WaitBoosterCoroutine());

            AmountPref--;
        }

        public void Purchase()
        {
            _spendAction.Spend(OnSuccess, null);
        }

        public void PurchaseAlt()
        {
            _altSpendAction.Spend(OnSuccessAlt, null);
        }

        public void OnTap(Action givePurchaseChoice)
        {
            if (AmountPref == 0)
            {
                givePurchaseChoice();
            }
            else
            {
                Apply();
            }
        }
    } 
}

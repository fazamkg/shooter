using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Faza
{
    [CreateAssetMenu]
    public class BoosterData : ScriptableObject
    {
        public event Action OnUpdated;
        public event Action OnApplied;

        [SerializeField] private string _id;
        [SerializeField] private string _title;
        [SerializeField, TextArea] private string _itemDescription;
        [SerializeField, TextArea] private string _windowDescription;
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

        public string Title => Localization.Get(_title);

        public string ItemDescription
        {
            get
            {
                return string.Format(Localization.Get(_itemDescription),
                    _purchaseCount, // 0
                    _boosterAmount, // 1
                    _altPurchaseCount, // 2
                    _altBoosterAmount, // 3
                    _spendAction.Cost, // 4
                    _altSpendAction.Cost, // 5
                    _duration); // 6
            }
        }

        public string WindowDescription
        {
            get
            {
                return string.Format(Localization.Get(_windowDescription),
                    _purchaseCount, // 0    
                    _boosterAmount, // 1
                    _altPurchaseCount, // 2
                    _altBoosterAmount, // 3
                    _spendAction.Cost, // 4
                    _altSpendAction.Cost, // 5
                    _duration); // 6
            }
        }

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

        public bool IsUnlocked
        {
            get => Storage.GetBool($"{_id}_unlocked");
            set => Storage.SetBool($"{_id}_unlocked", value);
        }

        public SpendAction MainSpendAction => _spendAction;
        public SpendAction AltSpendAction => _altSpendAction;

        public int MainPurchaseCount => _purchaseCount;
        public int AltPurchaseCount => _altPurchaseCount;

        public int BoosterAmount => _boosterAmount;
        public int AltBoosterAmount => _altBoosterAmount;

        public float Duration => _duration;

        private void OnSuccess()
        {
            PurchaseCountPref++;
            if (PurchaseCountPref >= _purchaseCount)
            {
                PurchaseCountPref = 0;
                AmountPref += _boosterAmount;
            }

            OnUpdated?.Invoke();
        }

        public void GiveFreeAmount()
        {
            AmountPref++;
            OnUpdated?.Invoke();
        }

        private void OnSuccessAlt()
        {
            AltPurchaseCountPref++;
            if (AltPurchaseCountPref >= _altPurchaseCount)
            {
                AltPurchaseCountPref = 0;
                AmountPref += _altBoosterAmount;
            }

            OnUpdated?.Invoke();
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
            if (_runningBoosters.Contains(this))
            {
                //Debug.Log("booster still running");
                return;
            }

            if (AmountPref <= 0) return;

            foreach (var effect in _effects)
            {
                effect.Apply();
            }

            _runningBoosters.Add(this);

            Routines.StartCoroutine_(WaitBoosterCoroutine());

            AmountPref--;

            OnUpdated?.Invoke();

            OnApplied?.Invoke();
        }

        public void Purchase()
        {
            _spendAction.Spend(OnSuccess, null);

            OnUpdated?.Invoke();
        }

        public void PurchaseAlt()
        {
            _altSpendAction.Spend(OnSuccessAlt, null);

            OnUpdated?.Invoke();
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

        public static bool AnyBoosterRunning()
        {
            return _runningBoosters.Count != 0;
        }

        public static bool IsBoosterRunning(BoosterData booster)
        {
            return _runningBoosters.Contains(booster);
        }

        public static void Init()
        {
            _runningBoosters.Clear();
        }
    } 
}

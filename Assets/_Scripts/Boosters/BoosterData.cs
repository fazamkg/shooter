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
        [SerializeField] private AudioClip _activationSound;
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
            get => Storage.GetInt(StorageKey.GetBoosterAmountKey(_id));
            private set => Storage.SetInt(StorageKey.GetBoosterAmountKey(_id), value);
        }

        public int PurchaseCountPref
        {
            get => Storage.GetInt(StorageKey.GetBoosterPurchaseCountKey(_id));
            private set => Storage.SetInt(StorageKey.GetBoosterPurchaseCountKey(_id), value);
        }

        public int AltPurchaseCountPref
        {
            get => Storage.GetInt(StorageKey.GetBoosterAltPurchaseCountKey(_id));
            private set => Storage.SetInt(StorageKey.GetBoosterAltPurchaseCountKey(_id), value);
        }

        public bool IsUnlocked
        {
            get => Storage.GetBool(StorageKey.GetBoosterUnlockedKey(_id));
            set => Storage.SetBool(StorageKey.GetBoosterUnlockedKey(_id), value);
        }

        public SpendAction MainSpendAction => _spendAction;
        public SpendAction AltSpendAction => _altSpendAction;

        public int MainPurchaseCount => _purchaseCount;
        public int AltPurchaseCount => _altPurchaseCount;

        public int BoosterAmount => _boosterAmount;
        public int AltBoosterAmount => _altBoosterAmount;

        public float Duration => _duration;

        public static void Init()
        {
            _runningBoosters.Clear();
        }

        public void GiveFreeAmount()
        {
            AmountPref++;
            OnUpdated?.Invoke();
        }

        public void Apply()
        {
            if (_runningBoosters.Contains(this))
            {
                return;
            }

            if (AmountPref <= 0) return;

            foreach (var effect in _effects)
            {
                effect.Apply();
            }

            FazaAudio.Play(_activationSound);

            _runningBoosters.Add(this);

            Routines.StartCoroutineNew(WaitBoosterCoroutine());

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
    } 
}

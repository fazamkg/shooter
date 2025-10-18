using System.Collections.Generic;
using UnityEngine;
using System;

namespace Faza
{
    public enum ModifierType
    {
        Flat,
        Add,
        Mult
    }

    [Serializable]
    public class Modifier
    {
        [SerializeField] private string _id;
        [SerializeField] private float _base;
        [SerializeReference] private List<Modifier> _flat = new();
        [SerializeReference] private List<Modifier> _add = new();
        [SerializeReference] private List<Modifier> _mul = new();

        private static Dictionary<string, Modifier> _all = new();

        public string Id => _id;

        public Modifier(string id, float baseValue)
        {
            _id = id;
            _base = baseValue;

            _all[id] = this;
        }

        public void Init()
        {
            _all[_id] = this;
        }

        public static Modifier Get(string id)
        {
            return _all[id];
        }

        public float Evaluate()
        {
            var flat = _base;
            for (var i = 0; i < _flat.Count; i++)
            {
                flat += _flat[i].Evaluate();
            }

            var add = 1f;
            for (var i = 0; i < _add.Count; i++)
            {
                add += _add[i].Evaluate();
            }

            var mul = 1f;
            for (var i = 0; i < _mul.Count; i++)
            {
                mul *= _mul[i].Evaluate();
            }

            return flat * add * mul;
        }

        public void AddModifier(ModifierType type, string id, float modifier)
        {
            AddModifier(type, new Modifier(id, modifier));
        }

        public void AddModifier(ModifierType type, Modifier modifier)
        {
            switch (type)
            {
                case ModifierType.Flat:
                    _flat.Add(modifier);
                    break;

                case ModifierType.Add:
                    _add.Add(modifier);
                    break;

                case ModifierType.Mult:
                    _mul.Add(modifier);
                    break;
            }
        }

        public void RemoveModifier(ModifierType type, string id)
        {
            var exist = _all.TryGetValue(id, out var mod);
            if (exist == false) return;

            RemoveModifier(type, mod);
        }

        public void RemoveModifier(ModifierType type, Modifier modifier)
        {
            switch (type)
            {
                case ModifierType.Flat:
                    _flat.Remove(modifier);
                    break;

                case ModifierType.Add:
                    _add.Remove(modifier);
                    break;

                case ModifierType.Mult:
                    _mul.Remove(modifier);
                    break;
            }
        }
    } 
}

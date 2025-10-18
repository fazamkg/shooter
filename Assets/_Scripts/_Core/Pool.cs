using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    public static class Pool
    {
        private static Dictionary<string, GameObject> _prefabs = new();
        private static Dictionary<string, Stack<GameObject>> _pools = new();

        public static void SetPrefab(string id, GameObject prefab)
        {
            _prefabs[id] = prefab;
            _pools[id] = new();
        }

        public static GameObject Get(string id)
        {
            var pool = _pools[id];
            var popped = pool.TryPop(out var instance);
            if (popped)
            {
                instance.SetActive(true);
                return instance;
            }

            var prefab = _prefabs[id];
            return Object.Instantiate(prefab);
        }

        public static void Release(string id, GameObject gameObject)
        {
            if (gameObject == null) return;

            var pool = _pools[id];
            gameObject.SetActive(false);
            pool.Push(gameObject);
        }

        public static void SetPrefab<T>(string id, T component) where T : Component
        {
            SetPrefab(id, component.gameObject);
        }

        public static T Get<T>(string id) where T : Component
        {
            return Get(id).GetComponent<T>();
        }

        public static void Release<T>(string id, T component) where T : Component
        {
            if (component == null) return;

            Release(id, component.gameObject);
        }
    } 
}

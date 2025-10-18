using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    public class Tracker : MonoBehaviour
    {
        [SerializeField] private string _id;

        private static Dictionary<string, GameObject> _objects = new();

        private void Awake()
        {
            _objects.Add(_id, gameObject);
        }

        private void OnDestroy()
        {
            _objects.Remove(_id);
        }

        public static GameObject Get(string id)
        {
            return _objects[id];
        }

        public static T Get<T>(string id) where T : Component
        {
            return Get(id).GetComponent<T>();
        }
    } 
}

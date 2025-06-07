using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    public class CameraTracker : MonoBehaviour
    {
        [SerializeField] private string _id;

        private static Dictionary<string, GameObject> _cameras = new();
        private static GameObject _lastActiveCamera;

        private void Awake()
        {
            _cameras.Add(_id, gameObject);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _cameras.Remove(_id);
        }

        public static void Activate(string id)
        {
            if (_lastActiveCamera != null) _lastActiveCamera.SetActive(false);
            var cam = _cameras[id];
            cam.SetActive(true);
            _lastActiveCamera = cam;
        }
    } 
}

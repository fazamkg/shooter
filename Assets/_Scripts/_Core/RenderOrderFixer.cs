using UnityEngine;

namespace Faza
{
    public class RenderOrderFixer : MonoBehaviour
    {
        [SerializeField] private int _renderQueue;

        private void Awake()
        {
            var renderer = GetComponent<Renderer>();
            renderer.material.renderQueue = _renderQueue;
        }
    } 
}

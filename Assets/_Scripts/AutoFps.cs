using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Faza
{
    public class AutoFps : MonoBehaviour
    {
        [SerializeField] private UniversalRenderPipelineAsset _settings;

        private int _frames;

        private void Awake()
        {
            StartCoroutine(Adjust());
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            _frames++;
        }

        private IEnumerator Adjust()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(4f);
                var fps = _frames / 4f;
                _frames = 0;

                if (fps < 28f && _settings.renderScale > 0f)
                {
                    _settings.renderScale -= 0.1f;
                }
                else if (fps > 28f && _settings.renderScale < 1f)
                {
                    _settings.renderScale += 0.1f;
                }
            }
        }
    } 
}

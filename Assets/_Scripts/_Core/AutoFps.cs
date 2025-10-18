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
            StartCoroutine(AdjustCoroutine());
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            _frames++;
        }

        private IEnumerator AdjustCoroutine()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(2f);
                var fps = _frames / 2f;
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

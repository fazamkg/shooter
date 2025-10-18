using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Faza
{
    public class AutoFps : MonoBehaviour
    {
        private const float CHECK_INTERVAL = 2f;
        private const float TARGET_FPS = 28f;
        private const float RENDER_SCALE_STEP = 0.1f;

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
                yield return new WaitForSecondsRealtime(CHECK_INTERVAL);
                var fps = _frames / CHECK_INTERVAL;
                _frames = 0;

                if (fps < TARGET_FPS && _settings.renderScale > 0f)
                {
                    _settings.renderScale -= RENDER_SCALE_STEP;
                }
                else if (fps > TARGET_FPS && _settings.renderScale < 1f)
                {
                    _settings.renderScale += RENDER_SCALE_STEP;
                }
            }
        }
    } 
}

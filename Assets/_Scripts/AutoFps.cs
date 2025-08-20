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
            yield return new WaitForSecondsRealtime(5f);

            var oldFps = _frames / 5f;

            if (oldFps >= 58f)
            {
                print("faza: fps is decent");
                yield break;
            }

            while (true)
            {
                _frames = 0;
                _settings.renderScale -= 0.25f;
                print("faza: decrease resolution");

                yield return new WaitForSecondsRealtime(5f);

                var newFps = _frames / 5f;

                if ((newFps - oldFps).Abs() < 2f) 
                {
                    print("faza: fps is capped or we are cpu bound");
                    // most likely cpu bound here OR frame capped (vsync or browser or smth)
                    // so let's stop decreasing resolution

                    _settings.renderScale += 0.25f;
                    yield break;
                }

                if (newFps >= 58f)
                {
                    print("faza: fps is decent");
                    yield break;
                }

                oldFps = newFps;
            }
        }
    } 
}

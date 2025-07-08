using System.Collections;
using UnityEngine;
using TMPro;

namespace Faza
{
    public class FpsCounter : MonoBehaviour
    {
        private TMP_Text _text;

        private int _fps;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            StartCoroutine(Count());
        }

        private void Update()
        {
            _fps++;
        }

        private IEnumerator Count()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1f);
                _text.text = _fps.ToString();
                _fps = 0;
            }
        }
    } 
}

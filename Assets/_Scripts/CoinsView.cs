using UnityEngine;
using TMPro;

namespace Faza
{
    public class CoinsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _texts;

        private void Awake()
        {
            Currency.OnCoinsAdded += Currency_OnCoinsAdded;

            foreach (var text in _texts)
            {
                text.text = Currency.Coins.ToString();
            }
        }

        private void OnDestroy()
        {
            Currency.OnCoinsAdded -= Currency_OnCoinsAdded;
        }

        private void Currency_OnCoinsAdded(float oldValue, float newValue)
        {
            foreach (var text in _texts)
            {
                text.text = Currency.Coins.ToString();
            }
        }
    } 
}

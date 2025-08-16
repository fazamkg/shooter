using TMPro;
using UnityEngine;

namespace Faza
{
    public class LocalizationText : MonoBehaviour
    {
        [SerializeField] private string _key;

        private void Start()
        {
            GetComponent<TMP_Text>().text = Localization.Get(_key);
        }
    } 
}

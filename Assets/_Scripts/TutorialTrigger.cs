using System;
using UnityEngine;

namespace Faza
{
    public class TutorialTrigger : MonoBehaviour
    {
        public static event Action OnEnter;

        private void OnTriggerEnter(Collider other)
        {
            OnEnter?.Invoke();
        }

        private void OnDestroy()
        {
            OnEnter = null;
        }
    } 
}

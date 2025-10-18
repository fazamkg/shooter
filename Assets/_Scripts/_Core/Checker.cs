using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Faza
{
    public class Checker : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            print(collision.collider.gameObject.name);
        }
    } 
}

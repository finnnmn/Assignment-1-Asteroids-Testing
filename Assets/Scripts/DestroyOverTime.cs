using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidGame
{
    public class DestroyOverTime : MonoBehaviour
    {
        [SerializeField] [Min(0)] float destroyTime = 2;

        private void Start()
        {
            //Destroy self after destroyTime seconds
            Destroy(gameObject, destroyTime);
        }
    }
}

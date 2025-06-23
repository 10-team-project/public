using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH;

namespace KSH
{
    public class Fatigue : MonoBehaviour
    {
        [SerializeField] private ResourceDegenerator resourceDegenerator;
        
        public float FatigueCur => resourceDegenerator.Resource.Cur;

        public void Sleep(float amount)
        {
            resourceDegenerator.Resource.Increase(amount);
        }
    }
}

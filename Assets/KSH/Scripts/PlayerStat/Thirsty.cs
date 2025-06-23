using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH;

namespace KSH
{
    public class Thirsty : MonoBehaviour
    {
        [SerializeField] private ResourceDegenerator resourceDegenerator;
        
        public float ThirstyCur => resourceDegenerator.Resource.Cur;


        public void Drink(float amount)
        {
            resourceDegenerator.Resource.Increase(amount);
        }
    }
}

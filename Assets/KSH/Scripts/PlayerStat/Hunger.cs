using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using KSH;

namespace KSH
{
    public class Hunger : MonoBehaviour
    {
        [SerializeField] private ResourceDegenerator resourceDegenerator;

        public float HungerCur => resourceDegenerator.Resource.Cur;
        
        public void Eat(float amount)
        {
            resourceDegenerator.Resource.Increase(amount);
        }

    }    
}


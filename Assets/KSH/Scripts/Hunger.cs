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

        private void Eat(float amount)
        {
            resourceDegenerator.Resource.Decrease(amount);
        }

    }    
}


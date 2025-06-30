using KSH;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

namespace NTJ
{
    public class testHP : MonoBehaviour
    {
        // HP에 추가
        public float CurrentHP
        {
            get => resource.Cur;
            set => resource.Cur = value;
        }


        // Hunger에 추가

        public void SetHunger(float value)
        {
            resourceDegenerator.Resource.Cur = value;
        }

        // Thirst에 추가
        public void SetThirst(float value)
        {
            resourceDegenerator.Resource.Cur = value;
        }

        // Fatigue에 추가
        public void SetFatigue(float value)
        {
            resourceDegenerator.Resource.Cur = value;
        }
    }
}
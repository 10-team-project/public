using KSH;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

namespace NTJ
{
    public class testHP : MonoBehaviour
    {
        // HP�� �߰�
        public float CurrentHP
        {
            get => resource.Cur;
            set => resource.Cur = value;
        }


        // Hunger�� �߰�

        public void SetHunger(float value)
        {
            resourceDegenerator.Resource.Cur = value;
        }

        // Thirst�� �߰�
        public void SetThirst(float value)
        {
            resourceDegenerator.Resource.Cur = value;
        }

        // Fatigue�� �߰�
        public void SetFatigue(float value)
        {
            resourceDegenerator.Resource.Cur = value;
        }
    }
}
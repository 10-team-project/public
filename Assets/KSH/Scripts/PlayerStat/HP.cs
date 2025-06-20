using System.Collections;
using System.Collections.Generic;
using KSH;
using UnityEngine;

namespace KSH
{
    public class HP : MonoBehaviour
    {
        [Header("Assign a script")]
        [Tooltip("No changes needed inside ResourceDecay")]
        [SerializeField] private ResourceDegenerator resourceDegenerator;
        [SerializeField] private Hunger hunger;
        [SerializeField] private Thirsty thirsty;
        [SerializeField] private Fatigue fatigue;
        [Header("Resource amount Limit")]
        [SerializeField] private float hungerLimit;
        [SerializeField] private float thirstyLimit;
        [SerializeField] private float fatigueLimit;
        [Header("HP Decay Interval")]
        [SerializeField] private float decayTimeFromHunger;
        [SerializeField] private float decayTimeFromThirsty;
        [SerializeField] private float decayTimeFromFatigue;
        [Header("HP Drain Based on Resource Limits")]
        [SerializeField] private float HpLossFromHunger;
        [SerializeField] private float HpLossFromThirsty;
        [SerializeField] private float HpLossFromFatigue;
        
        public Hunger Hunger => hunger;
        public Thirsty Thirsty => thirsty;
        public Fatigue Fatigue => fatigue;

        private void Start()
        {
            resourceDegenerator.Resource.Cur = resourceDegenerator.Resource.Max; //체력 초기화
        }

        private void HealthDecay() //Hunger, Thirsty, Fatigue의 정해진 수치 이하가 되면 체력 떨어짐
        {
            if (hunger.HungerCur <= hungerLimit) // Hunger의 Cur이 일정 수준 내려가면
            {
                resourceDegenerator.ResourceTick(HpLossFromHunger, decayTimeFromHunger); // 몇초마다 HP 깎임   
            }
            else if (thirsty.ThirstyCur <= thirstyLimit) // Thirsty의 Cur이 일정 수준 내려가면
            {
                resourceDegenerator.ResourceTick(HpLossFromThirsty, decayTimeFromThirsty); //몇초마다 HP 깎임
            }
            else if (fatigue.FatigueCur <= fatigueLimit) // Fatigue의 Cur이 일정 수준 내려가면
            {
                resourceDegenerator.ResourceTick(HpLossFromFatigue, decayTimeFromFatigue); //몇초마다 HP 깎임
            }
        }
    }
}

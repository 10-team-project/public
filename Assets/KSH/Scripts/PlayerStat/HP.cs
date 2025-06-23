using System.Collections;
using System.Collections.Generic;
using KSH;
using UnityEngine;

namespace KSH
{
    public class HP : MonoBehaviour
    {
        [Header("Assign a script")] [Tooltip("No changes needed inside ResourceDecay")] 
        [SerializeField] private Resource resource;
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

        private float hungerTimer = 0f;
        private float thirstyTimer = 0f;
        private float fatigueTimer = 0f;

        private void Start()
        {
            resource.Cur = resource.Max; //체력 초기화
        }

        private void Update()
        {
            HealthDecay();
        }

        private void HealthDecay() //Hunger, Thirsty, Fatigue의 정해진 수치 이하가 되면 체력 떨어짐
        {
            if (hunger.HungerCur <= hungerLimit) // Hunger의 Cur이 일정 수준 내려가면
            {
                hungerTimer += Time.deltaTime;
                if (hungerTimer >= decayTimeFromHunger) // 정해진 시간이 타이머를 넘기면
                {
                    hungerTimer = 0f;
                    resource.Decrease(HpLossFromHunger); // 감소
                }
            }
            if (thirsty.ThirstyCur <= thirstyLimit) // Thirsty의 Cur이 일정 수준 내려가면
            {
                thirstyTimer += Time.deltaTime;
                if (thirstyTimer >= decayTimeFromThirsty)
                {
                    thirstyTimer = 0f;
                    resource.Decrease(HpLossFromThirsty);
                }
            }
            if (fatigue.FatigueCur <= fatigueLimit) // Fatigue의 Cur이 일정 수준 내려가면
            {
                fatigueTimer += Time.deltaTime;
                if (fatigueTimer >= decayTimeFromFatigue)
                {
                    fatigueTimer = 0f;
                    resource.Decrease(HpLossFromFatigue);
                }
            }
        }
    }
}

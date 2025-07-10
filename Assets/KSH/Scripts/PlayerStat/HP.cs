using System.Collections;
using System.Collections.Generic;
using KSH;
using UnityEngine;
using UnityEngine.UI;

namespace KSH
{
    public class HP : MonoBehaviour
    {
        [SerializeField] private ResourceDegenerator resourceDegenerator;
        [Header("Assign a script")] [Tooltip("No changes needed inside ResourceDecay")] 
        [SerializeField] private Resource resource;
        public Resource Resource => resource;[SerializeField] private Hunger hunger;
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
        [Header("UI")]
        [SerializeField] public Slider HpBarSlider;
        
        private float hungerTimer = 0f;
        private float thirstyTimer = 0f;
        private float fatigueTimer = 0f;

        private void Start()
        {
            resource.Cur = resource.Max; //체력 초기화
            resourceDegenerator.Resource.OnResourceChanged += OnHpChanged;
        }
        
        private void Update()
        {
            HealthDecay();
        }
        
        private void OnDestroy() => resourceDegenerator.Resource.OnResourceChanged -= OnHpChanged;
        
        private void OnHpChanged(Resource resource, float oldValue, float newValue) => HpBar();

        private void HpBar()
        {
            if (HpBarSlider != null)
                HpBarSlider.value = resource.Cur / resource.Max;
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

        public void Heal(float amount) => resourceDegenerator.Resource.Increase(amount);
        
        public void Damage(float amount) => resourceDegenerator.Resource.Decrease(amount);

        public float CurrentHP
        {
            get => resource.Cur;
            set => resource.Cur = value;
        }

        public float MaxHP
        {
            get => resource.Max;
        }
    }
}

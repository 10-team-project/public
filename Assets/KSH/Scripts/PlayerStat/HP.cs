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
        public Resource Resource => resource;
        [Header("UI")]
        [SerializeField] public Slider HpBarSlider;

        private void Start()
        {
            resource.Cur = resource.Max; //체력 초기화
            resourceDegenerator.Resource.OnResourceChanged += OnHpChanged;
        }
        
        private void OnDestroy() => resourceDegenerator.Resource.OnResourceChanged -= OnHpChanged;
        
        private void OnHpChanged(Resource resource, float oldValue, float newValue) => HpBar();

        private void HpBar()
        {
            if (HpBarSlider != null)
                HpBarSlider.value = resource.Cur / resource.Max;
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

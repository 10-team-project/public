using System;
using UnityEngine;
using UnityEngine.UI;

namespace KSH
{
    public class Thirsty : MonoBehaviour
    {
        [SerializeField] private ResourceDegenerator resourceDegenerator;
        [SerializeField] public Slider ThirstySlider;

        public float ThirstyCur => resourceDegenerator.Resource.Cur;
        public float ThirstyMax => resourceDegenerator.Resource.Max;
        public Resource Resource => resourceDegenerator.Resource;

        private void Start()
        {
            SetThirst(ThirstyMax);
            resourceDegenerator.Resource.OnResourceChanged += OnThirstyChanged;
        }

        private void OnDestroy() => resourceDegenerator.Resource.OnResourceChanged -= OnThirstyChanged;
        
        private void OnThirstyChanged(Resource resource, float oldValue, float newValue) => ThirstyUI();

        private void ThirstyUI()
        {
            if(ThirstySlider != null)
                ThirstySlider.value = ThirstyCur / ThirstyMax;
        }

        public void Drink(float amount) => resourceDegenerator.Resource.Increase(amount);
        public void Dehydrate(float amount) => resourceDegenerator.Resource.Decrease(amount);

        public void SetThirst(float value) => resourceDegenerator.Resource.Cur = value;
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace KSH
{
    public class Hunger : MonoBehaviour
    {
        [SerializeField] private ResourceDegenerator resourceDegenerator;
        [SerializeField] private Slider HungerSlider;

        public float HungerCur => resourceDegenerator.Resource.Cur;
        public float HungerMax => resourceDegenerator.Resource.Max;
        public Resource Resource => resourceDegenerator.Resource;

        private void Start()
        {
            SetHunger(HungerMax);
            resourceDegenerator.Resource.OnResourceChanged += OnHungerChanged;
        }

        private void OnDestroy() => resourceDegenerator.Resource.OnResourceChanged -= OnHungerChanged;
        
        private void OnHungerChanged(Resource resource, float oldValue, float newValue) => HungerUI();

        private void HungerUI()
        {
            if (HungerSlider != null)
                HungerSlider.value = HungerCur / HungerMax;
        }

        public void Eat(float amount) => resourceDegenerator.Resource.Increase(amount);
        public void Starve(float amount) => resourceDegenerator.Resource.Decrease(amount);

        public void SetHunger(float value) => resourceDegenerator.Resource.Cur = value;
    }
}


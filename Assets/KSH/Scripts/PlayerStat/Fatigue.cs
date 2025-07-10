using UnityEngine;
using UnityEngine.UI;

namespace KSH
{
    public class Fatigue : MonoBehaviour
    {
        [SerializeField] private ResourceDegenerator resourceDegenerator;
        [SerializeField] public Slider FatigueSlider;

        public float FatigueCur => resourceDegenerator.Resource.Cur;
        public float FatigueMax => resourceDegenerator.Resource.Max;
        public Resource Resource => resourceDegenerator.Resource;

        private void Start()
        {
            SetFatigue(FatigueMax);
            resourceDegenerator.Resource.OnResourceChanged += OnFatigueChanged;
        }
        
        private void OnDestroy() => resourceDegenerator.Resource.OnResourceChanged -= OnFatigueChanged;

        private void OnFatigueChanged(Resource resource, float oldValue, float newValue) => FatigueUI();

        private void FatigueUI()
        {
            if (FatigueSlider != null)
                FatigueSlider.value = FatigueCur / FatigueMax;
        }

        public void Sleep(float amount) => resourceDegenerator.Resource.Increase(amount);
        public void WakeUp(float amount) => resourceDegenerator.Resource.Decrease(amount);

        public void SetFatigue(float value) => resourceDegenerator.Resource.Cur = value;
    }
}

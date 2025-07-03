using UnityEngine;
using UnityEngine.UI;

namespace KSH
{
    public class Thirsty : MonoBehaviour
    {
        [SerializeField] private ResourceDegenerator resourceDegenerator;
        [SerializeField] private Slider ThirstySlider;

        public float ThirstyCur => resourceDegenerator.Resource.Cur;
        public float ThirstyMax => resourceDegenerator.Resource.Max;

        private void Start() => SetThirst(ThirstyMax);
        
        private void Update() => ThirstyUI();

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

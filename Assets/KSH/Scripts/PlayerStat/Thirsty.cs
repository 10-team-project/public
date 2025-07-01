using UnityEngine;

namespace KSH
{
    public class Thirsty : MonoBehaviour
    {
        [SerializeField] private ResourceDegenerator resourceDegenerator;

        public float ThirstyCur => resourceDegenerator.Resource.Cur;


        public void Drink(float amount)
        {
            resourceDegenerator.Resource.Increase(amount);
        }

        public void SetThirst(float value)
        {
            resourceDegenerator.Resource.Cur = value;
        }
    }
}

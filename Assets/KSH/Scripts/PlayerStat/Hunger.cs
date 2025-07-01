using UnityEngine;

namespace KSH
{
    public class Hunger : MonoBehaviour
    {
        [SerializeField] private ResourceDegenerator resourceDegenerator;

        public float HungerCur => resourceDegenerator.Resource.Cur;

        public void Eat(float amount)
        {
            resourceDegenerator.Resource.Increase(amount);
        }

        public void SetHunger(float value)
        {
            resourceDegenerator.Resource.Cur = value;
        }
    }
}


using UnityEngine;

namespace KSH
{
    public class Fatigue : MonoBehaviour
    {
        [SerializeField] private ResourceDegenerator resourceDegenerator;

        public float FatigueCur => resourceDegenerator.Resource.Cur;

        public void Sleep(float amount)
        {
            resourceDegenerator.Resource.Increase(amount);
        }

        public void SetFatigue(float value)
        {
            resourceDegenerator.Resource.Cur = value;
        }
    }
}

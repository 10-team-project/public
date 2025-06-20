using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH;

namespace KSH
{
    public class ResourceDegenerator : MonoBehaviour, ITickableResource
    {
        [Header("Decreases resource amount over time")]
        [Header("Resource Decay")]
        [SerializeField] private float decayTime; // 줄어들 시간(몇초마다 줄어들지)
        [SerializeField] private float resourceAmount; //줄어들 자원 양
        [SerializeField] private bool decaying = true;
        
        [Header("Resource")]
        [SerializeField] private Resource resource;
        public Resource Resource => resource;
    
        public float DecayTime => decayTime;
        public float ResourceAmount => resourceAmount;
        public bool Decaying => decaying;

        private float timer = 0f; //초기 시간
        
        public void ResourceTick(float amount, float decreaseTime)
        {
            amount = resourceAmount;
            decreaseTime = decayTime;
            timer += Time.deltaTime;
            if (timer >= decreaseTime && decaying) // 만약 시간이 설정된 시간보다 크거나 같고 true이면
            {
                timer = 0f; //다시 초기화
                resource.Decrease(amount); //감소
            }
        }
    }
}

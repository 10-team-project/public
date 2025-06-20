using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH;

namespace KSH
{
    public class Hunger : MonoBehaviour, ITickableResource
    {
        [Header("Resource Decay")]
        [SerializeField] private float decayTime; // 줄어들 시간(몇초마다 줄어들지)
        [SerializeField] private float HungerAmount; //배고픔 양
        [SerializeField] private bool decaying = true;
        [Header("Resource Script")]
        [SerializeField] private Resource resource;
        
        public float DecayTime => decayTime;
        public float ResourceAmount => HungerAmount;
        public bool Decaying => decaying;

        private float timer = 0f; //초기 시간
        
        public void ResourceTick()
        {
            timer += Time.deltaTime;
            if (timer >= DecayTime && Decaying) // 만약 시간이 설정된 시간보다 크거나 같고 true이면
            {
                timer = 0f; //다시 초기화
                resource.Increase(HungerAmount); //증가
            }
        }

        public void Eat(float amount) // 음식 먹을 때 배고픔 수치가 떨어지게
        {
            resource.Decrease(amount);
        }
    }    
}


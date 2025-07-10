using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH;
using NTJ;

namespace KSH
{
    public class ResourceDegenerator : MonoBehaviour, ITickableResource
    {
        [Header("Decreases resource amount over time")]
        [Header("Resource Decay")]
        [SerializeField] private float decayTime; // 줄어들 시간(몇초마다 줄어들지)
        [SerializeField] private float resourceAmount; //줄어들 자원 양
        [SerializeField] private float timedecrease; //날짜당 줄어들 시간
        [SerializeField] private bool decaying = true;
        
        [Header("Resource")]
        [SerializeField] private Resource resource;
        
        public Resource Resource => resource;
    
        public float DecayTime => decayTime;
        public float ResourceAmount => resourceAmount;
        public bool Decaying => decaying;

        private float timer = 0f; //초기 시간
        private int day = 0;

        private void Start()
        {
            day = GameTimeManager.Instance.CurrentDay;
        }
        
        private void Update()
        {
            int curday = GameTimeManager.Instance.CurrentDay;

            if (curday != day)
            {
                timer = 0f;
                day = curday;
            }
            ResourceTick();
        }
        
        public void ResourceTick()
        {
            if (GameTimeManager.Instance == null || resource == null) return;

            int curDay = GameTimeManager.Instance.CurrentDay;
            timer += Time.deltaTime;
            
            float decreaseTime = Mathf.Max(decayTime - timedecrease * curDay, 0f);
            
            if (timer >= decreaseTime && decaying) // 만약 시간이 설정된 시간보다 크거나 같고 true이면
            {
                timer = 0f; //다시 초기화
                resource.Decrease(resourceAmount); //감소
            }
        }
    }
}

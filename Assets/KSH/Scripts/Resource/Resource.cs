using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using KSH;

namespace KSH
{
    public class Resource : MonoBehaviour, IResource
    {
        [Header("Resource Amount")]
        [Range(0, 140)][SerializeField] private float max;//최대 자원 양
        [Range(0, 140)] [SerializeField] private float cur; //현재 자원 양
        
        public event Action<Resource, float, float> OnResourceChanged; //콜백

        public float Max
        {
            get => max;
            set => max = value;
        }
        public float Cur
        {
            get => cur;
            set
            {
                if (Mathf.Approximately(cur, value)) return; //현재 값과 value값이 거의 같으면 반환
                float beforeCur = cur; // 감소하기 전 자원 양 저장
                cur = Mathf.Clamp(value, 0, max); // 최소, 최대 제한 값 설정
                if (Mathf.Abs(beforeCur - cur) > 0.01f) 
                {
                    OnResourceChanged?.Invoke(this, beforeCur, cur); // 현재 자원양과 전의 양의 차이가 생기면 이벤트 발생
                }
            }
        }

        public void Decrease(float amount) //자원 감소
        {
            Cur -= amount;
        }
        public void Increase(float amount) //자원 증가
        {
            Cur += amount;
        }
    }
}


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
        
        public event Action<Resource> OnResourceChanged; //코드용 이벤트
        
        public float Max => max;
        public float Cur
        {
            get => cur;
            set => cur = value;
        }

        public void Decrease(float amount) //자원 감소
        {
            float beforeCur = cur; // 감소하기 전 자원 양 저장
            cur = Mathf.Clamp(cur - amount, 0, max); // 최소, 최대 제한 값 설정
            if (Mathf.Abs(beforeCur - cur) > 0.01f) 
            {
                OnResourceChanged?.Invoke(this); // 현재 자원양과 전의 양의 차이가 생기면 이벤트 발생
            }
        }
        public void Increase(float amount) //자원 증가
        {
            float beforeCur = cur; // 증가하기 전 자원 양 저장
            cur = Mathf.Clamp(cur + amount, 0, max); // 최소, 최대 제한 값 설정
            if (Mathf.Abs(cur - beforeCur) > 0.01f) 
            {
                OnResourceChanged?.Invoke(this); // 현재 자원양과 전의 양의 차이가 생기면 이벤트 발생
            }
        }
    }
}


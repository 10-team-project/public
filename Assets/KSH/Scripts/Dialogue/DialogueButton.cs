using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UI;
using KSH;
using System;

namespace KSH
{
    public class DialogueButton : MonoBehaviour, IInteractableUI
    {
        [SerializeField] private Button talkButton;
        [SerializeField] private Button eventButton;
        [SerializeField] private GameObject mainDialogue;
        [SerializeField] private GameObject NPC;
        [SerializeField] private GameObject eventBubbleIcon;
        
        [Header("Interact Radius")]
        [SerializeField] private float radius; //상호작용할 원의 범위
    
        private bool isreach = false;
        private bool previousisreach = false;
        private bool isEvent = false;
        
        public event Action<bool> OnInteract; // bool 이벤트

        private void Start()
        {
            talkButton.onClick.AddListener(() =>
            {
                ScriptManager.Instance.StartScript(12203);
                mainDialogue.SetActive(false);
            });
            eventButton.onClick.AddListener(() =>
            {
                ScriptManager.Instance.StartScript(11701);
                mainDialogue.SetActive(false);
                isEvent = false;
                UpdateEvent();
            });
            
            OnInteract += Interact; // 이벤트에 연결
            
            ScriptManager.Instance.OnEnd += () => //대화가 끝나면
            {
                if(isreach) //만약 범위 안에 있으면
                    Interact(true);
            };
            UpdateEvent();
        }

        private void Update()
        {
            Detect();
        }
        
        public void Detect()
        {
            isreach = false;
            Vector3 center = NPC.transform.position; // NPC오브젝트의 위치를 센터로 함 
            
            Collider[] colliders = Physics.OverlapSphere(center, radius); //원모양의 콜라이더 생성
    
            foreach (Collider col in colliders)
            {
                if (col.gameObject.layer == LayerMask.NameToLayer("Player")&& !isreach) // 콜라이더에 감지된 오브젝트의 레이어가 "Player" 이거나 isreach = false라면
                {
                    isreach = true;
                    break;
                } 
            }
    
            if (previousisreach != isreach) //전의 bool 값과 현재 bool값이 다르면
            {
                OnInteract?.Invoke(isreach); // 이벤트 호출
                previousisreach = isreach;
            }

            if (!isreach && isEvent)
            {
                eventBubbleIcon?.SetActive(true);
            }
        }
        
        public void Interact(bool state) //bool값에 따른 상호작용
        {
            if (!ScriptManager.Instance.IsTalk)
            {
                mainDialogue.SetActive(state);
                talkButton.gameObject.SetActive(state);
                eventButton.gameObject.SetActive(isEvent);
                eventBubbleIcon.gameObject.SetActive(false);
            }
        }

        [EditorAttributes.Button("Event Trigger")]
        public void TriggerEvent() //이벤트발동
        {
            isEvent = true;
            UpdateEvent();
        }

        private void UpdateEvent()
        {
            eventBubbleIcon?.SetActive(isEvent);
            eventButton?.gameObject.SetActive(false);
        }
    
        private void OnDrawGizmosSelected() //기즈모
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(NPC.transform.position, radius);
        }
    }
}
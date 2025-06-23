using System;
using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using KSH;

namespace KSH
{
    public class ItemTest : MonoBehaviour //아이템 사용되는지 확인하는 테스트
    {
        [Header("Item Test")]
        public GameObject usableItem;
    
        private KSH.IUsable usable;
    
        public event Action<KSH.IUsable> OnItemUsed;

        void Start()
        {
            usable = usableItem.GetComponent<KSH.IUsable>(); 
        }
        
        [Button("Item Test")]
        public void ItemUse()
        {
            if (usable != null) //컴포넌트가 null이 아니면
            {
                usable.Use(); //아이템 사용
                OnItemUsed?.Invoke(usable); //이벤트 발생
                Destroy(usableItem); //없어짐
                Debug.Log($"{usableItem.name}이 사용되었습니다.");
            }
        }
    }
}

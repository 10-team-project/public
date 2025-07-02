using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH;

namespace KSH
{
    public class BaseController : MonoBehaviour, InextNode //다양한 노드(대사, 선택지 등)을 처리하는 기능
    {
        [SerializeField] protected NodeType nodeType; //노드 타입

        protected bool onNode; //출력되는 노드 확인

        public void Awake()
        {
            ScriptManager.AddNextNode(this); //시작 시 ScriptManager에 추가
        }

        public virtual void NextNode(BaseNode b)
        {
            if (b == null) //스크립트 매니저에게 전달 받은 노드가 Null이면
            {
                onNode = false;
                return; //종료
            }
            if (b.nodetype != nodeType) //현재 노드와 전달된 노드가 다르면
            {
                onNode = false;
                return; //종료
            }
            onNode = true; //일치하면 True
        }
    }

    public interface InextNode
    {
        void NextNode(BaseNode b);
    }
}
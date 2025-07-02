using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH;

namespace KSH
{
    public class BaseController : MonoBehaviour, InextNode
    {
        [SerializeField] protected NodeType nodeType;

        protected bool onNode; //출력되는 노드 확인

        public void Awake()
        {
            ScriptManager.AddNextNode(this);
        }

        public virtual void NextNode(BaseNode b)
        {
            if (b == null)
            {
                onNode = false;
                return;
            }
            if (b.nodetype != nodeType) //현재 노드와 전달된 노드가 다르면
            {
                onNode = false;
                return;
            }
            onNode = true;
        }
    }

    public interface InextNode
    {
        void NextNode(BaseNode b);
    }
    
}
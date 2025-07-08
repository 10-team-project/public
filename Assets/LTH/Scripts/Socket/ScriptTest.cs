using System.Collections;
using System.Collections.Generic;
using KSH;
using TMPro;
using UnityEngine;

namespace LTH
{
    public class ScriptTest : MonoBehaviour, InextNode
    {
        public TMP_Text dialogueText;

        private void Awake()
        {
            Debug.Log("ScriptTest: Awake ½ÇÇàµÊ, AddNextNode È£Ãâ");
            ScriptManager.AddNextNode(this);
        }

        public void NextNode(BaseNode node)
        {
            Debug.Log("ScriptTest: NextNode È£ÃâµÊ");

            if (node == null)
            {
                dialogueText.text = "";
                return;
            }

            if (node is DialogueNode d)
            {
                Debug.Log($"ScriptTest: DialogueNode µµÂø - allId: {d.allId}, dialogue: {d.dialogue}");
                dialogueText.text = $"{d.allId}: {d.dialogue}";
            }
        }
    }
}
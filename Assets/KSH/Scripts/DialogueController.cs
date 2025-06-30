using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : BaseController
{
    public Image leftportrait;
    public Image rightportrait;
    public GameObject dialoguePanel; // 대사 오브젝트
    public GameObject leftnamePanel; //이름 오브젝트
    public GameObject rightnamePanel;
    public TMP_Text leftnameText; //이름텍스트
    public TMP_Text rightnameText;
    public TMP_Text dialogueText; // 대사 텍스트
    public string leftcharacter; //왼쪽에 배치될 캐릭터

    public override void NextNode(BaseNode b)
    {
        base.NextNode(b);
        if (b==null || b.nodetype != nodeType)
        {
            dialoguePanel.SetActive(false);
            leftnamePanel.SetActive(false);
            rightnamePanel.SetActive(false);
            leftportrait.gameObject.SetActive(false);
            rightportrait.gameObject.SetActive(false);
            return;
        }
        
        dialoguePanel.SetActive(true);
        
        DialogueNode dNode = b as DialogueNode;
        if(dNode == null) return;
        
        leftnameText.text = dNode.name;
        rightnameText.text = dNode.name;
        dialogueText.text = dNode.dialogue;
        
        var data = CharacterManager.Instance.GetCharacterData(dNode.name);
        if (data != null)
        {
            leftportrait.sprite = data.sprite;
            rightportrait.sprite = data.sprite;
        }

        bool isLeft = (dNode.name == leftcharacter);
        
        leftportrait.gameObject.SetActive(isLeft);
        leftnamePanel.SetActive(isLeft);
        rightportrait.gameObject.SetActive(!isLeft);
        rightnamePanel.SetActive(!isLeft);

        if (isLeft)
        {
            leftnameText.text = dNode.name;
            leftportrait.sprite = data.sprite;
        }
        else
        {
            rightnameText.text = dNode.name;
            rightportrait.sprite = data.sprite;
        }
    }

    private void Update()
    {
        if (!onNode) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            onNode = false;
            ScriptManager.Instance.NextNode();
        }
    }
}

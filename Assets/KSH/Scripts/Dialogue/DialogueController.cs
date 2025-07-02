using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    public float delay;

    private float timer = 0f;

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

        string charname = CharacterManager.Instance.GetCharacterData(dNode.allId);
        
        leftnameText.text = charname;
        rightnameText.text = charname;
       
       StartCoroutine(TypeTextEffect(dialogueText, dNode.dialogue));

       Sprite portrait = CharacterManager.Instance.GetPortraitData(dNode.allId);
        if (portrait != null)
        {
            leftportrait.sprite = portrait;
            rightportrait.sprite = portrait;
        }

        bool isLeft = (charname == leftcharacter);
        
        leftportrait.gameObject.SetActive(isLeft);
        leftnamePanel.SetActive(isLeft);
        rightportrait.gameObject.SetActive(!isLeft);
        rightnamePanel.SetActive(!isLeft);

        if (isLeft)
        {
            leftnameText.text = charname;
            leftportrait.sprite = portrait;
        }
        else
        {
            rightnameText.text = charname;
            rightportrait.sprite = portrait;
        }
    }

    IEnumerator TypeTextEffect(TMP_Text text, string fulltext) //텍스트 타이핑 효과
    {
        text.text = string.Empty;
        
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < fulltext.Length; i++)
        {
            stringBuilder.Append(fulltext[i]);
            text.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Update()
    {
        if (!onNode) return;
        
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            onNode = false;
            timer = 0f;
            ScriptManager.Instance.NextNode();
        }
        else if (timer >= delay)
        {
            onNode = false;
            timer = 0f;
            ScriptManager.Instance.NextNode();       
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public DialogueData[] dialogueDatas;
    
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    
    public CharacterSprite characterSprite;
    
    Dictionary<int, List<Dictionary<string,object>>> scriptDic = new Dictionary<int, List<Dictionary<string, object>>>();

    private void Start()
    {
        ScriptManager.Instance.StartScript(0);
    }
    
    int curDialogueIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }
    }

    public void NextDialogue()
    {
        if(dialogueDatas.Length <= curDialogueIndex) return; 
        
        nameText.text = dialogueDatas[curDialogueIndex].name; //UI에 이름 표시
        dialogueText.text = dialogueDatas[curDialogueIndex].dialogue; //UI에 대사 표시
        
        characterSprite.ChangeSprite(dialogueDatas[curDialogueIndex].characterSprite); //대사에 따른 이미지 변경
        curDialogueIndex++;
    }
    
}

[System.Serializable]
public class DialogueData
{
    public Sprite characterSprite; //캐릭터 이미지
    public string name; //캐릭터 이름
    public string dialogue; //대사
}

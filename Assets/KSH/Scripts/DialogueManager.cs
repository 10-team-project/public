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
        var datas = CSVParser.Parse("CSV/TestDialogue"); // CSV파일 파싱

        int curId = 0; //현재ID
        for (int i = 0; i < datas.Count; i++) //행을 하나씩 반복
        {
            if (!string.IsNullOrEmpty(datas[i]["ID"].ToString()) && (int.TryParse(datas[i]["ID"].ToString(),out int id))) // ID칸이 비어있지 않고 ID의 값을 정수로 변환가능하면 id에 저장
            {
                if (!scriptDic.ContainsKey(id)) // 만약 scriptDic에 id키가 없으면
                {
                    curId = id; //현재 키를 id로 저장
                    scriptDic.Add(curId, new List<Dictionary<string, object>>()); //새로운 리스트로 추가
                }
            }
            scriptDic[curId].Add(datas[i]); // 현재 줄을 현재 id에 해당하는 리스트에 추가
        }
        
        characterSprite = FindObjectOfType<CharacterSprite>(); //CharacterSprite 타입의 컴포넌트 가져옴
        nameText.text = "";
        dialogueText.text = "";
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

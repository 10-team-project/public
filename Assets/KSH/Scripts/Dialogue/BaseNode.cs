using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using KSH;

namespace KSH
{
    public enum NodeType
{
    dialogue, //대사
    option //선택지
}
[System.Serializable]
public abstract class BaseNode
{
    public NodeType nodetype;

    protected int nextID = -1;
    protected int saveAllID;

    public virtual int GetNextID()
    {
        return nextID;
    }
}

[System.Serializable]
public class DialogueNode : BaseNode
{
    public string dialogue; //대사
    public string allId;
    
    public DialogueNode(Dictionary<string, object> datas)
    {
        nodetype = NodeType.dialogue; //노드타입을 dialogue로 설정
        
        allId = datas["CharacterEmotion"].ToString();
        if (!string.IsNullOrEmpty(allId))
            saveAllID = int.Parse(allId);
        
        dialogue = datas["Dialogue"].ToString(); //대사를 문자열로 저장
        string reference = datas["Reference"].ToString(); // 선택지 번호를 문자열로 저장
        
        if(!string.IsNullOrEmpty(reference)) //만약 reference 값이 비어있지않다면
            nextID = int.Parse(reference); //정수로 변환해서 nextID에 저장
    }
}

public class OptionNode : BaseNode
{
    public string allId;
    public OptionData[] optionDatas;

    public OptionNode(Dictionary<string, object> datas)
    {
        nodetype = NodeType.option; //노드타입을 option으로 저장

        allId = datas["CharacterEmotion"].ToString();
        if (!string.IsNullOrEmpty(allId))
            saveAllID = int.Parse(allId);
        
        string[] optionTexts = datas["Dialogue"].ToString().Split('/'); // /로 분리
        string[] optionNextIDs = datas["Reference"].ToString().Split('/');

        if (optionTexts.Length != optionNextIDs.Length) //선택지 갯수와 다음 노드 갯수가 다르면 
        {
            Debug.LogError("옵션 선택에 따른 다른 대화 설정 지정 오류");
            return;
        }
        
        optionDatas = new OptionData[optionTexts.Length]; //옵션데이터 배열 생성
        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionDatas[i] = new OptionData(optionTexts[i], int.Parse(optionNextIDs[i])); //정수인 다음 아이디로 옵션 데이터 객체 생성
        }
    }

    public class OptionData
    {
        public string text { private set; get; }
        public int nextID { private set; get; }

        public OptionData(string t, int n)
        {
            text = t;
            nextID = n;
        }
    }
}
}
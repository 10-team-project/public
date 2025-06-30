using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

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

    public virtual int GetNextID()
    {
        return nextID;
    }
}

[System.Serializable]
public class DialogueNode : BaseNode
{
    public string name;
    public string dialogue;
    
    public DialogueNode(Dictionary<string, object> datas)
    {
        nodetype = NodeType.dialogue; //노드타입을 dialogue로 설정
        
        name = datas["Name"].ToString(); //이름을 문자열로 저장
        dialogue = datas["Dialogue"].ToString();
        string reference = datas["Reference"].ToString();
        
        if(!string.IsNullOrEmpty(reference)) //만약 reference 값이 비어있지않다면
            nextID = int.Parse(reference); //정수로 변환해서 nextID에 저장
    }
}

public class OptionNode : BaseNode
{
    public string name;
    public OptionData[] optionDatas;

    public OptionNode(Dictionary<string, object> datas)
    {
        nodetype = NodeType.option; //노드타입을 option으로 저장
        
        name = datas["Name"].ToString(); //이름을 문자열로 저장
        
        string[] optionTexts = datas["Dialogue"].ToString().Split('/');
        string[] optionNextIDs = datas["Reference"].ToString().Split('/');

        if (optionTexts.Length != optionNextIDs.Length)
        {
            Debug.LogError("옵션 선택에 따른 다른 대화 설정 지정 오류");
            return;
        }
        
        optionDatas = new OptionData[optionTexts.Length];
        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionDatas[i] = new OptionData(optionTexts[i], int.Parse(optionNextIDs[i]));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using KSH;

namespace KSH
{
    public class ScriptManager
{
    private static ScriptManager instance;
    public static ScriptManager Instance //싱글톤
    {
        get
        {
            if (instance == null)
                instance = new ScriptManager();

            return instance;
        }
    }

    Dictionary<int, List<BaseNode>> scriptDataDic = new Dictionary<int, List<BaseNode>>();

    public ScriptManager()
    {
        var datas = CSVParser.Parse("CSV/TestDialogue"); //CSV 파싱

        int curId = 0;
        NodeType lastNodeType = NodeType.dialogue; //이전 값
        for (int i = 0; i < datas.Count; i++) //행을 하나씩 반복
        {
            if (int.TryParse(datas[i]["ID"].ToString(), out int id)) //만약 ID의 값을 정수로 변환 가능하면 id에 저장
            {
                if (!scriptDataDic.ContainsKey(id)) //Dictionary에 id키가 없다면
                {
                    curId = id; //현재 아이디로 지정
                    scriptDataDic.Add(curId, new List<BaseNode>()); //새로운 리스트 추가
                }
            }

            int nodeTypeInt;
            string nodeTypestr = datas[i]["NodeType"]?.ToString(); //NodeType값을 문자열로 저장

            if (string.IsNullOrWhiteSpace(nodeTypestr)) //만약 비어있거나 공백이라면
            {
                nodeTypeInt = (int)lastNodeType; //lastNodeType 값으로 사용
            }
            else
            {
                nodeTypeInt = int.TryParse(nodeTypestr, out var temp) ? temp : (int)lastNodeType;//nodeTypestr을 정수로 변환 시도    
            }
            
            NodeType nodeType = (NodeType)nodeTypeInt; //enum으로 변환
            lastNodeType = nodeType; //lastNodeType에 저장
            
            switch (nodeType)
            {
                case NodeType.dialogue:
                    scriptDataDic[curId].Add(new DialogueNode(datas[i])); //ID에 해당하는 리스트에 추가
                    break;
                case NodeType.option:
                    scriptDataDic[curId].Add(new OptionNode(datas[i]));
                    break;
            }
        }
    }
    
    private static List<InextNode> nextNodes = new List<InextNode>();
    public static void AddNextNode(InextNode n) => nextNodes.Add(n); //노드 오브젝트 추가
    public static void RemoveNextNode(InextNode n) => nextNodes.Remove(n); //제거
    public static void NotifyNextNode(BaseNode b) => nextNodes.ForEach(n => n.NextNode(b)); //Next노드 호출

    int curId;
    int curOrder;

    public void StartScript(int id)
    {
        curId = id;
        curOrder = -1;
        NextNode();
    }

    public void NextNode()
    {
        curOrder++; //순서 이동

        if (curOrder >= scriptDataDic[curId].Count) //현재 순서가 현재 아이디의 수보다 크거나 같으면
        {
            int nextIdx = scriptDataDic[curId][curOrder - 1].GetNextID(); //마지막에 실행한 노드의 다음 아이디를 가져옴

            if (nextIdx == 0 || !scriptDataDic.ContainsKey(nextIdx)) //nextIdx가 0이거나 유효하지 않으면
            {
                NotifyNextNode(null); // Null 처리
                return;
            }
            else
            {
                StartScript(nextIdx); //다음 ID로 시작
            }
            return;
        }
        NotifyNextNode(scriptDataDic[curId][curOrder]); //현재ID와 현재 순서 전달
    }
}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    [SerializeField] private string csv_DFileName;
    [SerializeField] private string csv_SFileName;
    
    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>(); //데이터 꺼낼 때 int값으로 꺼내옴
    Dictionary<int, SelectDialogue> selectDic = new Dictionary<int, SelectDialogue>();

    public static bool isFinish = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DialogueParser theParser = GetComponent<DialogueParser>();
            Dialogue[] dialogues = theParser.Parse(csv_DFileName);
            for (int i = 0; i < dialogues.Length; i++)
            {
                dialogueDic.Add(i+1, dialogues[i]);
            }
            isFinish = true;
        }
    }

    public Dialogue[] GetDialogue(int _StartNum, int _EndNum)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        for (int i = 0; i <= _EndNum - _StartNum; i++)
        {
            dialogueList.Add(dialogueDic[_StartNum + i]);
        }
        
        return dialogueList.ToArray();
    }
    
    public SelectDialogue[] GetSelectDialogue(int _StartNum, int _EndNum)
    {
        List<SelectDialogue> dialogueList = new List<SelectDialogue>();

        for (int i = 0; i <= _EndNum - _StartNum; i++)
        {
            dialogueList.Add(selectDic[_StartNum + i]);
        }
        
        return dialogueList.ToArray();
    }
}

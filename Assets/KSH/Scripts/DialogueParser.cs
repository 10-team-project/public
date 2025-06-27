using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); //대사 리스트 생성
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName); //csv파일 가져옴
        
        string[] data = csvData.text.Split(new char[]{'\n'}); //엑셀 엔터 단위로 나눠주기

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[]{','}); //콤마 단위로 쪼개져서 row로 들어감
            
            Dialogue dialogue = new Dialogue(); //대사 리스트 생성

            dialogue.Charactername = row[1];
            
            List<string> contextList = new List<string>();
            List<string> EventList = new List<string>();
            List<string> SkipList = new List<string>();

            do
            {
                contextList.Add(row[2]);
                EventList.Add(row[3]);
                SkipList.Add(row[4]);
                
                if (++i < data.Length)
                {
                    row = data[i].Split(new char[]{','});
                }
                else
                {
                    break;
                }
            } while (row[0].ToString() == "");
            
            dialogue.contexts = contextList.ToArray();
            dialogue.EventNum = EventList.ToArray();
            dialogue.SkipNum = SkipList.ToArray();
            
            dialogueList.Add(dialogue);
        }
        return dialogueList.ToArray();
    }
    
    public SelectDialogue[] SParse(string _CSVFileName)
    {
        List<SelectDialogue> dialogueList = new List<SelectDialogue>(); //대사 리스트 생성
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName); //csv파일 가져옴
        
        string[] data = csvData.text.Split(new char[]{'\n'}); //엑셀 엔터 단위로 나눠주기

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[]{','}); //콤마 단위로 쪼개져서 row로 들어감
            
            SelectDialogue selectdialogue = new SelectDialogue(); //대사 리스트 생성

            List<string> contextList = new List<string>();
            List<string> nextList = new List<string>();

            do
            {
                contextList.Add(row[2]);
                nextList.Add(row[3]);
                    
                if (++i < data.Length)
                {
                    row = data[i].Split(new char[]{','});
                }
                else
                {
                    break;
                }
            } while (row[0].ToString() == "");
            
            selectdialogue.contexts = contextList.ToArray();
            selectdialogue.NextNum = nextList.ToArray();
            dialogueList.Add(selectdialogue);
        }
        return dialogueList.ToArray();
    }
}

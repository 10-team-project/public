using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionController : BaseController
{
    public GameObject uiObj;
    
    public OptionPanel[] optionPanels;
    
    OptionNode curOptionNode;

    public override void NextNode(BaseNode b)
    {
        if (b == null)
        {
            onNode = false;
            return;
        }
        base.NextNode(b);
        if (b.nodetype != nodeType)
        {
            uiObj.SetActive(false);
            return;
        }
        
        uiObj.SetActive(true);
        curOptionNode = b as OptionNode;

        string allId = curOptionNode.allId;
        for (int i = 0; i < optionPanels.Length; i++)
        {
            if (i < curOptionNode.optionDatas.Length) //현재 노드의 선택지 수보다 작으면
            {
                optionPanels[i].gameObject.SetActive(true); //버튼 활성화
                optionPanels[i].SetOptionData(curOptionNode.optionDatas[i], result =>
                {
                    onNode = false;
                    ScriptManager.Instance.StartScript(result.nextID);
                });
            }
            else
            {
                optionPanels[i].gameObject.SetActive(false);
            }
        }
    }
}

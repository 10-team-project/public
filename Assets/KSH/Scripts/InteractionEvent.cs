using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] private DialogueEvent dialogue;
    [SerializeField] private SelectEvent select;

    public Dialogue[] GetDialogue()
    {
        dialogue.dialogues = DatabaseManager.instance.GetDialogue((int)dialogue.line.x, (int)dialogue.line.y);
        return dialogue.dialogues;
    }

    public SelectDialogue[] GetSelectDialogue()
    {
        select.selectdialogues = DatabaseManager.instance.GetSelectDialogue((int)select.line.x, (int)select.line.y);
        return select.selectdialogues;
    }
    
}

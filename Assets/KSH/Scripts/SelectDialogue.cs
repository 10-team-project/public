using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectDialogue
{
    [Tooltip("SelectDialogue")] 
    public string[] contexts;
    
    [Tooltip("Next Number")]
    public string[] NextNum;
}

[System.Serializable]
public class SelectEvent
{
    public string name;
    
    public Vector2 line;
    public SelectDialogue[] selectdialogues;
}

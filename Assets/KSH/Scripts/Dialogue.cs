using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("TalkingCharacter")]
    public string Charactername;

    [Tooltip("Dialogue")] 
    public string[] contexts;
    
    [Tooltip("Event Number")]
    public string[] EventNum;

    [Tooltip("Skip Number")] 
    public string[] SkipNum;
}

[System.Serializable]
public class DialogueEvent
{
    public string name;
    
    public Vector2 line;
    public Dialogue[] dialogues;
}


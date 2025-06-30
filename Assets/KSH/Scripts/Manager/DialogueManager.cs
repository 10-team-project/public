using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject go_dialogueBar;
    [SerializeField] private GameObject go_dialogueNameBar;
    
    [SerializeField] private TMP_Text txt_dialogue;
    [SerializeField] private TMP_Text txt_Name;

    [SerializeField] private float AutoTime;
    
    Dialogue[] dialogues;
    
    private bool isDialogue = false; //대화 중일 경우 true
    private bool isNext = false; //특정 키 입력 대기
    
    private int lineCount = 0; //대화 카운트
    private int contextCount = 0; //대사 카운트

    private void Update()
    {
        if (isDialogue && isNext)
        {
             if (Input.GetKeyDown(KeyCode.Space))
             {
                 NextDialogue();
             }
        }
    }

    public void NextDialogue()
    {
        isNext = false;
        txt_dialogue.text = "";
        if (++contextCount < dialogues[lineCount].contexts.Length)
        {
            StartCoroutine(TypeWriter());   
        }
        else
        {
            contextCount = 0;
            if (++lineCount < dialogues.Length)
            {
                StartCoroutine(TypeWriter());
            }
            else
            {
                EndDialogue();
            }
        }
    }
    
    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        isDialogue = true;
        txt_dialogue.text = "";
        txt_Name.text = "";
        dialogues = p_dialogues;

        StartCoroutine(TypeWriter());
    }

    private void EndDialogue()
    {
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        SettingUI(false);
    }

    IEnumerator TypeWriter()
    {
        SettingUI(true);

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount];
        t_ReplaceText = t_ReplaceText.Replace("'", ",");
        t_ReplaceText = t_ReplaceText.Replace("|", "\n"); // '|'를 줄바꿈으로
        
        txt_dialogue.text = t_ReplaceText;
        txt_Name.text = dialogues[lineCount].Charactername;
        isNext = true;
        yield return null;

        StartCoroutine(Autonext(AutoTime));
    }

    IEnumerator Autonext(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isDialogue && isNext)
        {
            NextDialogue();
        }
    }

    private void SettingUI(bool p_flag)
    {
        go_dialogueBar.SetActive(p_flag);
        go_dialogueNameBar.SetActive(p_flag);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns;

namespace KSH
{
    public class DialogueManager :  SingletonBehaviour<DialogueManager> //싱글톤
    {
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private GameObject NamePanel;
        [SerializeField] private Text dialogueText;
        [SerializeField] private Text NameText;
    }   
}

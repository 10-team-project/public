using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteracioncontroller : MonoBehaviour
{
   [SerializeField] private DialogueManager dialogueManager;
   [SerializeField] Dialogue[] testDialogues; //test

   [SerializeField] InteractionEvent interactionEvent; //테스트

   private void Start()
   {
      dialogueManager = FindObjectOfType<DialogueManager>();
   }

   void Update()
   {
      if (Input.GetKeyDown(KeyCode.E))
      {
         Dialogue[] dialogues = interactionEvent.GetDialogue(); //테스트
         dialogueManager.ShowDialogue(dialogues); //괄호 안에 상호작용하는 대상을 판별해서 그 대상에 있는 대사 내용 꺼내오기 적어야함.
      }
   }
   
   //나중에 대화할 때 주변 UI가 사라지게 하고싶으면 코드적기
}

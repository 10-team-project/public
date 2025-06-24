using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LTH;

namespace LTH
{
    public class NPCInteraction : MonoBehaviour, IInteractable, IInputLockHandler
    {
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TMP_Text dialogueText;

        [TextArea]
        [SerializeField] private string message = "Welcome To My Game";

        public void Interact()
        {
            if (InputManager.instance.IsBlocked(InputType.Interaction)) return;

            InputManager.instance.StartInput(this);
            dialogueText.text = message;
            dialoguePanel.SetActive(true);
        }

        private void Update()
        {
            if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
            {
                dialoguePanel.SetActive(false);
                InputManager.instance.EndInput(this);
            }
        }

        public bool IsInputBlocked(InputType inputType)
        {
            return inputType == InputType.Move;
        }

        public bool OnInputEnd()
        {
            return true;
        }

        public bool OnInputStart()
        {
            return true;
        }
    }
}

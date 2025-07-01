using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UI;
public class DialogueButton : MonoBehaviour
{
    [SerializeField] private Button talkButton;
    [SerializeField] private Button eventButton;
    [SerializeField] private GameObject mainDialogue;

    private void Start()
    {
        talkButton.onClick.AddListener(() =>
        {
            ScriptManager.Instance.StartScript(10);
            mainDialogue.SetActive(false);
        });
        eventButton.onClick.AddListener(() =>
        {
            ScriptManager.Instance.StartScript(0);
            mainDialogue.SetActive(false);
        });
    }
}

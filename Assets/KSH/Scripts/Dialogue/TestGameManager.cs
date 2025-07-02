using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH;

namespace KSH
{
    public class TestGameManager : MonoBehaviour
    {
        public GameObject mainDialogue;
        void Start()
        {
            mainDialogue.SetActive(false);
            //ScriptManager.Instance.StartScript(0);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                mainDialogue.SetActive(true);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTH;

namespace LTH
{
    public enum InputType
    {
        Move,
        Interaction,
        UI,
        All
    }


    public class InputManager : MonoBehaviour
    {
        public static InputManager instance { get; private set; }

        private readonly List<IInputLockHandler> activeLocks = new();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartInput(IInputLockHandler startInputRequest)
        {
            if (!activeLocks.Contains(startInputRequest))
            {
                activeLocks.Add(startInputRequest);
                startInputRequest.OnInputStart();
            }
        }

        public void EndInput(IInputLockHandler endInputRequest)
        {
            if (activeLocks.Contains(endInputRequest))
            {
                activeLocks.Remove(endInputRequest);
                endInputRequest.OnInputEnd();
            }
        }

        public bool IsBlocked(InputType type)
        {
            foreach (var handler in activeLocks)
            {
                if (handler.IsInputBlocked(type)) return true;
            }
            return false;
        }
    }
}

using System.Collections.Generic;
using Patterns;

namespace LTH
{
    public enum InputType
    {
        Move,
        Interaction,
        UI,
        All
    }

    public class InputManager : SingletonBehaviour<InputManager>
    {
        private readonly List<IInputLockHandler> activeLocks = new();

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

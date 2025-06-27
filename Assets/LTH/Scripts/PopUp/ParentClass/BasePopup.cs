using UnityEngine;

namespace LTH
{
    public abstract class BasePopup : MonoBehaviour, IInputLockHandler
    {
        public virtual void Open()
        {
            InputManager.Instance.StartInput(this);
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            InputManager.Instance.EndInput(this);
            gameObject.SetActive(false);
        }

        public virtual bool IsInputBlocked(InputType inputType)
        {
            return inputType == InputType.Interaction || inputType == InputType.Move;
        }

        public virtual bool OnInputStart() => true;
        public virtual bool OnInputEnd() => true;
    }

}

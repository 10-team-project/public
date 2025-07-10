using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTH;

namespace SHG
{
  public class KeyboardInputBlocker : MonoBehaviour, IInputLockHandler
  {
    void OnEnable()
    {
      App.Instance.InputManager.StartInput(this); 
    }

    void OnDisable()
    {
      App.Instance.InputManager.EndInput(this);
    }

    public bool IsInputBlocked(InputType inputType)
    {
      return (true);
    }

    public bool OnInputStart()
    {
      return (true);
    }

    public bool OnInputEnd()
    {
      return (true);
    }
  }
}

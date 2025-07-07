using System.Collections;
using System.Collections.Generic;
using LTH;
using UnityEngine;

namespace SHG
{
  public class PlayerInputLocker : MonoBehaviour, IInputLockHandler
  {
    public bool IsInputBlocked(InputType inputType)
    {
      return (true);
    }

    public bool OnInputEnd()
    {
      return (true);
    }

    public bool OnInputStart()
    {
      return (true);
    }

    void OnEnable()
    {
      App.Instance.ScriptManager.OnStart += this.LockInput;      
      App.Instance.ScriptManager.OnEnd += this.ReleaseInput;
    }

    void OnDisable()
    {
      App.Instance.ScriptManager.OnStart -= this.LockInput;      
      App.Instance.ScriptManager.OnEnd -= this.ReleaseInput;
    }

    void LockInput()
    {
      App.Instance.InputManager.StartInput(this);
    }

    void ReleaseInput()
    {
      App.Instance.InputManager.EndInput(this);
    }
  }

}

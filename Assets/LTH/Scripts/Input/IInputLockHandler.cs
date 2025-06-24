using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTH;
public interface IInputLockHandler
{
    public bool IsInputBlocked(InputType inputType);
    public bool OnInputStart();
    public bool OnInputEnd();
}

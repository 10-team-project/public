using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInteractableUI
{
    public void Detect();
    public void Interact(bool state);
    
    public event Action<bool> OnInteract;
}

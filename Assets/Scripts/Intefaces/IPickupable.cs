using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public interface IPickupable
{
    public event Action<Item> OnPickedUp;
    public void Pickup();
}

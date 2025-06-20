using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using LTH;

namespace LTH
{
    public interface IPickupable
    {
        public event Action<Item> OnPickedUp;
        public void Pickup();
    }
}




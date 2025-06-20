using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTH;

namespace LTH
{
    public class Item : MonoBehaviour, IInteractable
    {
       [HideInInspector] public string itemName;

        public void Interact()
        {
            if (this is IPickupable pickupable)
            {
                pickupable.Pickup();
            }
        }
    }
}


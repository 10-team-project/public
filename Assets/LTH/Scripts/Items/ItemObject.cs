using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemObject : MonoBehaviour, IInteractable, IPickupable
{
    [SerializeField] private ItemData itemData;
    public event Action<Item> OnPickedUp;

    private Item itemInstance;

    private void Awake()
    {
        itemInstance = new SomeConcreteItem(itemData);
    }

    public void Interact()
    {
        Pickup();
    }

    public void Pickup()
    {
        Debug.Log($"{itemInstance.Data.Name} ¿ª(∏¶) »πµÊ«ﬂΩ¿¥œ¥Ÿ.");
        OnPickedUp?.Invoke(itemInstance);
        Destroy(gameObject);
    }

}


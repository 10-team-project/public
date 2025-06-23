using System;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable, IPickupable
{
  public event Action<Item> OnPickedUp;
  private Item item;

  public void SetItem(Item item) 
  {
    this.item = item;
  }

  public void Interact()
  {
    Pickup();
  }

  public void Pickup()
  {
    Debug.Log($"{item.Data.Name} ��(��) ȹ���߽��ϴ�.");
    OnPickedUp?.Invoke(item);
    Destroy(gameObject);
  }

}


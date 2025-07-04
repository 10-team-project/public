using System;
using UnityEngine;
using EditorAttributes;
using SHG;

public class ItemObject : MonoBehaviour, IInteractable, IPickupable
{
  public event Action<Item> OnPickedUp;
  [SerializeField]
  Item item;
  PlayerItemController player;

  public void SetItem(Item item) 
  {
    this.item = item;
  }

  public void Interact()
  {
    if (this.player == null) {
      this.player = GameObject.FindWithTag("Player")?.GetComponent<PlayerItemController>();
    }
    if (this.player == null) {
      Pickup();
    }
    else {
      this.player.LootItem(this);
    }
  }

  public void Pickup()
  {
    //Debug.Log($"{item.Data.Name} ¿ª(∏¶) »πµÊ«ﬂΩ¿¥œ¥Ÿ.");
    OnPickedUp?.Invoke(item);
    Destroy(gameObject);
  }
}


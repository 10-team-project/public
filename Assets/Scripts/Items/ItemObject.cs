using System;
using UnityEngine;
using EditorAttributes;
using SHG;
using KSH;

public class ItemObject : MonoBehaviour, IInteractable, IPickupable
{
  public event Action<Item> OnPickedUp;
  [SerializeField]
  Item item;
  PlayerItemController player;
  [SerializeField]
  GameObject floatingUI;

  public void SetItem(Item item) 
  {
    this.item = item;
  }

  public void Interact()
  {
    if (this.floatingUI != null) {
      this.floatingUI.gameObject.SetActive(false);
    }
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


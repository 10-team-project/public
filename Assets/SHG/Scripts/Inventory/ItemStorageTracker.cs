using System;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class ItemTracker: IObservableObject<ItemTracker>
  {
    HashSet<ItemData> ObtainedItems;
    HashSet<ItemData> UsedItems;
    [SerializeField] [ReadOnly]
    List<ItemData> newObtainedItems;
    [SerializeField] [ReadOnly]
    List<ItemData> newUsedItems;

    public Action<ItemTracker> WillChange { get; set; }
    public Action<ItemTracker> OnChanged { get; set; }

    public ItemTracker(Inventory inventory)
    {
      this.ObtainedItems = new ();
      this.UsedItems = new ();
      this.newObtainedItems = new ();
      this.newUsedItems = new ();
      this.RegisterEvents(inventory);
    }

    void RegisterEvents(Inventory inventory)
    {
      inventory.OnUseItem += this.OnItemUse;
      inventory.OnObtainItem += this.OnObtainItem;
    }

    void ConsumeNewUsedItems(Action<List<ItemData>> handler)
    {
      this.WillChange?.Invoke(this);
      handler.Invoke(this.newUsedItems);
      this.newUsedItems.Clear();
      this.OnChanged?.Invoke(this);
    }

    void ConsumeNewObtainedItems(Action<List<ItemData>> handler)
    {
      this.WillChange?.Invoke(this);
      handler.Invoke(this.newObtainedItems);
      this.newObtainedItems.Clear();
      this.OnChanged?.Invoke(this);
    }

    void OnObtainItem(ItemData item) 
    {
      if (!this.ObtainedItems.Contains(item)) {
        this.WillChange?.Invoke(this);
        this.newObtainedItems.Add(item); 
        this.ObtainedItems.Add(item);
        this.OnChanged?.Invoke(this);
      }
    }

    void OnItemUse(ItemData item)
    {
      if (!this.UsedItems.Contains(item)) {
        this.WillChange?.Invoke(this);
        this.newUsedItems.Add(item); 
        this.UsedItems.Add(item);
        this.OnChanged?.Invoke(this);
      }
    }
  }
}


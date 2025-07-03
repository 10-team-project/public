using System;
using System.Collections.Generic;
using UnityEngine;

namespace SHG
{
  public class ItemTracker: IObservableObject<ItemTracker>
  {
    HashSet<ItemData> obtainedItems;
    HashSet<ItemData> usedItems;
    [SerializeField] 
    public List<ItemData> NewObtainedItems { get; private set; }
    [SerializeField] 
    public List<ItemData> NewUsedItems { get; private set; }

    public Action<ItemTracker> WillChange { get; set; }
    public Action<ItemTracker> OnChanged { get; set; }

    public ItemTracker(Inventory inventory)
    {
      this.obtainedItems = new ();
      this.usedItems = new ();
      this.NewObtainedItems = new ();
      this.NewUsedItems = new ();
      this.RegisterEvents(inventory);
    }

    void RegisterEvents(Inventory inventory)
    {
      inventory.OnUseItem += this.OnItemUse;
      inventory.OnObtainItem += this.OnObtainItem;
    }

    public void ConsumeNewUsedItems(Action<List<ItemData>> handler)
    {
      this.WillChange?.Invoke(this);
      handler.Invoke(this.NewUsedItems);
      this.NewUsedItems.Clear();
      this.OnChanged?.Invoke(this);
    }

    public void ConsumeNewObtainedItems(Action<List<ItemData>> handler)
    {
      this.WillChange?.Invoke(this);
      handler.Invoke(this.NewObtainedItems);
      this.NewObtainedItems.Clear();
      this.OnChanged?.Invoke(this);
    }

    void OnObtainItem(ItemData item) 
    {
      if (!this.obtainedItems.Contains(item)) {
        this.WillChange?.Invoke(this);
        this.NewObtainedItems.Add(item); 
        this.obtainedItems.Add(item);
        this.OnChanged?.Invoke(this);
      }
    }

    void OnItemUse(ItemData item)
    {
      if (!this.usedItems.Contains(item)) {
        this.WillChange?.Invoke(this);
        this.NewUsedItems.Add(item); 
        this.usedItems.Add(item);
        this.OnChanged?.Invoke(this);
      }
    }
  }
}


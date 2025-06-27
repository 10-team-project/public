using System;
using System.Collections.Generic;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class ItemStorageBase: IObservableObject<ItemStorageBase>
  {

    public virtual int MAX_STACK_COUNT => 20;
    public virtual int MAX_SLOT_COUNT => 20;
    public Dictionary<ItemData, int> Items { get; protected set; }
    public Action<ItemStorageBase> WillChange { get; set; }
    public Action<ItemStorageBase> OnChanged { get; set; }

  #if UNITY_EDITOR
    [SerializeField]
    public List<string> ItemNamesForDebugging;
  #endif

    protected ItemStorageBase()
    {
      this.Items = new ();
    #if UNITY_EDITOR
      this.ItemNamesForDebugging = new();
    #endif
    }

    public bool IsAbleToAddItem(ItemAndCount itemAndCount)
    {
      var currentSlotCount = this.CountUsedSlot();
      if (currentSlotCount < MAX_SLOT_COUNT) {
        return (true);
      }
      if (currentSlotCount == MAX_SLOT_COUNT) {
        var currentItemCount = this.GetItemCount(itemAndCount.Item);
        if ((currentItemCount % MAX_STACK_COUNT) + itemAndCount.Count <
          MAX_STACK_COUNT) {
          return (true);
        }
      }
      return (false);
    }

    public int CountUsedSlot()
    {
      int count = 0;   
      foreach (var itemAndCount in this.Items) {
        var itemCount = itemAndCount.Value;
        if (itemCount % MAX_STACK_COUNT == 0) {
          count += (itemCount / MAX_STACK_COUNT);
        }
        else {
          count += (itemCount / MAX_STACK_COUNT) + 1;
        }
      }
      return (count);
    }

    public void AddItem(Item item)
    {
      this.WillChange?.Invoke(this);
#if UNITY_EDITOR
      this.AddItemName(item.Data);
      if (!this.IsAbleToAddItem(new ItemAndCount { Item = item.Data, Count = 1})) {
        throw (new ApplicationException($"Unable to add more {item.Data.Name}"));
      }
#endif
      if (this.Items.TryGetValue(item.Data, out int itemCount)) {
        this.Items[item.Data] = itemCount + 1;
      }
      else {
        this.Items.Add(item.Data, 1);
      }
      this.OnChanged?.Invoke(this);
    }

#if UNITY_EDITOR
    void AddItemName(ItemData data) 
    {
      this.ItemNamesForDebugging.Add(data.Name);
      this.ItemNamesForDebugging.Sort();
    }
#endif


    public int GetItemCount(ItemData itemData)
    {
      if (this.Items.TryGetValue(itemData, out int itemCount)) {
        return (itemCount);
      }
      else {
        return (0);
      }
    }

#if UNITY_EDITOR
    void RemoveItemName(ItemData itemData)
    {
      var index = this.ItemNamesForDebugging.FindIndex(
        name => name == itemData.Name);
      if (index != -1) {
        this.ItemNamesForDebugging.RemoveAt(index);
      }
    }
#endif

    public Item GetItem(ItemData itemData)
    {
      int itemCount = this.GetItemCount(itemData);
      if (itemCount < 1) {
        throw (new ApplicationException($"GetItem: No {itemData.Name} in Inventory")); 
      }
      this.WillChange?.Invoke(this);
      Item item = Item.CreateItemFrom(itemData);
#if UNITY_EDITOR
      this.RemoveItemName(itemData);
#endif
      if (itemCount > 0) {
        this.Items.Remove(itemData);
      }
      else {
        this.Items[itemData] = itemCount - 1;
      }
      this.OnChanged?.Invoke(this);
      return (item);
    }

    public Item PeakItem(ItemData itemData)
    {
      int itemCount = this.GetItemCount(itemData);
      if (itemCount < 1) {
        throw (new ApplicationException($"GetItem: No {itemData.Name} in Inventory")); 
      }

      return (Item.CreateItemFrom(itemData));
    }
  }
}

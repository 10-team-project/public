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
    public const string ITEM_DIR = "Assets/SHG/Test/Items";
    public Action<ItemData> OnObtainItem;

  #if UNITY_EDITOR
    [SerializeField]
    public List<string> ItemNamesForDebugging;
  #endif

    public List<ItemSaveData> GetItemSaveDataList()
    {
      List<ItemSaveData> list = new (this.Items.Count);
      
      foreach (var itemAndCount in this.Items) {
        if (itemAndCount.Value > 0) {
          list.Add(new ItemSaveData {id = itemAndCount.Key.Id, count = itemAndCount.Value});
        }
      }
      return (list);
    }

    public void LoadFromItemSaveDataList(List<ItemSaveData> data)
    {
       
    }

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
      if ( currentSlotCount == MAX_SLOT_COUNT) {
        var currentItemCount = this.GetItemCount(itemAndCount.Item);
        if (currentSlotCount != 0 &&
          currentSlotCount % this.MAX_STACK_COUNT != 0) {
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
        if (itemAndCount.Key.IsStoryItem) {
          count += itemAndCount.Value;
        }
        else if (itemCount % MAX_STACK_COUNT == 0) {
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
      Debug.Log($"item add {item.Data.Name}");
      if (!this.IsAbleToAddItem(new ItemAndCount { Item = item.Data, Count = 1})) {
        throw (new ApplicationException($"Unable to add more {item.Data.Name}"));
      }
      this.AddItemName(item.Data);
#endif
      if (this.Items.TryGetValue(item.Data, out int itemCount)) {
        this.Items[item.Data] = itemCount + 1;
      }
      else {
        this.Items.Add(item.Data, 1);
      }
      this.OnObtainItem.Invoke(item.Data);
      this.OnChanged?.Invoke(this);
    }

    public void AddItems(Item item, int count)
    {
      if (count < 1) {
        throw (new ArgumentException($"Add {item.Data.Name} {count}"));
      }
      if (item.Data.IsStoryItem) {
        throw (new ApplicationException($"Stroy items are not able to added at once"));
      }
      this.WillChange?.Invoke(this);
      #if UNITY_EDITOR
      for (int i = 0; i < count; i++) {
        this.AddItemName(item.Data);
      }
      if (!this.IsAbleToAddItem(new ItemAndCount { Item = item.Data, Count = count})) {
        throw (new ApplicationException($"Unable to add {count} more {item.Data.Name}"));
      }
      #endif
      if (this.Items.TryGetValue(item.Data, out int itemCount)) {
        this.Items[item.Data] = itemCount + count;
      }
      else {
        this.Items.Add(item.Data, count);
      }
      this.OnObtainItem.Invoke(item.Data);
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

    public (Item item, int count) GetItems(ItemData itemData, int count)
    {
      int itemCount = this.GetItemCount(itemData);
      if (itemCount < count) {
        throw (new ApplicationException($"GetItem: Not enough {itemData.Name} required: {count} ")); 
      }
      this.WillChange?.Invoke(this);
      Item item = Item.CreateItemFrom(itemData);
      #if UNITY_EDITOR
      for (int i = 0; i < count; i++) {
        this.RemoveItemName(itemData);
      }
      #endif
      if (itemCount < count + 1) {
        this.Items.Remove(itemData);
      }
      else {
        this.Items[itemData] = itemCount - count;
      }
      this.OnChanged?.Invoke(this);
      return (item, count);
    }

    public void RemoveItem(ItemData itemData, int count)
    {
      int itemCount = this.GetItemCount(itemData);
      if (itemCount < count) {
        #if UNITY_EDITOR
        throw (new ApplicationException($"RemoveItem")); 
        #else
        return ;
        #endif
      }
      this.WillChange?.Invoke(this);
      if (itemCount - count < 1) {
        this.Items.Remove(itemData);
      }
      else {
        this.Items[itemData] = itemCount - count;
      }
      this.OnChanged?.Invoke(this);
    }

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
      if (itemCount < 2) {
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

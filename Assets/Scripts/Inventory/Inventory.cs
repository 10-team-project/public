using System;
using System.Collections.Generic;
using UnityEngine;
using Patterns;

public class Inventory : SingletonBehaviour<Inventory>, IObservableObject<Inventory>
{

  Dictionary<ItemData, int> items;
#if UNITY_EDITOR
  [SerializeField]
  public List<string> ItemNamesForDebugging;
#endif

  public Action<Inventory> WillChange { get; set; }
  public Action<Inventory> OnChanged { get; set; }

  protected override void Awake()
  {
    base.Awake();
    this.items = new ();
#if UNITY_EDITOR
    this.ItemNamesForDebugging = new();
#endif
  }

  public void AddItem(ItemData itemData)
  {
    this.WillChange?.Invoke(this);
#if UNITY_EDITOR
    this.AddItemName(itemData);
#endif
    if (this.items.TryGetValue(itemData, out int itemCount)) {
      this.items[itemData] = itemCount + 1;
    }
    else {
      this.items.Add(itemData, 1);
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
    if (this.items.TryGetValue(itemData, out int itemCount)) {
      return (itemCount);
    }
    else {
      return (0);
    }
  }

  public GameObject GetItemAsGameObject(ItemData itemData)
  {
    if (itemData.Prefab == null) {
      throw (new ArgumentException($"{itemData.Name} has no Prefab"));
    }
    Item item = this.GetItem(itemData);
    var gameObject = Instantiate(itemData.Prefab);
    var itemHolder = gameObject.AddComponent<ItemHolder>();
    itemHolder.ItemData = itemData;
    return (gameObject);
  }

  public Item GetItem(ItemData itemData)
  {
    int itemCount = this.GetItemCount(itemData);
    if (itemCount < 1) {
      throw (new ApplicationException($"GetItem: No {itemData.Name} in Inventory")); 
    }
    this.WillChange?.Invoke(this);
    Item item;
    if (itemData is RecoveryItemData recoveryItemData) {
      item = new RecoveryItem(recoveryItemData); 
    }
    else {
      throw (new NotImplementedException());
    }
#if UNITY_EDITOR
    var index = this.ItemNamesForDebugging.FindIndex(
      name => name == itemData.Name);
    if (index != -1) {
      this.ItemNamesForDebugging.RemoveAt(index);
    }
#endif
    if (itemCount > 0) {
      this.items.Remove(itemData);
    }
    else {
      this.items[itemData] = itemCount - 1;
    }
    this.OnChanged?.Invoke(this);
    return (item);
  }
}

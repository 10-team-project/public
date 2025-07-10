using System;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class InventoryTest : MonoBehaviour
  {
    [SerializeField]
    ItemData itemToAdd;
    [SerializeField]
    ItemData itemToGet;
    [SerializeField]
    ItemData itemToMoveQuickSlot;
    [SerializeField]
    ItemData itemToGetFromQuickSlot;
    [SerializeField, ReadOnly]
    string inventorySlot;

    [Button ("Test add item")]
    void AddItemTest()
    {
      if (this.itemToAdd == null) {
        Debug.Log("itemToAdd is none");
        return ;
      }
      var item = Item.CreateItemFrom(this.itemToAdd);
      App.Instance.Inventory.AddItem(item);
    }

    [Button ("Test get item")]
    void GetItemTest()
    {
      if (this.itemToGet == null) {
        Debug.Log("itemToGet is none");
        return ;
      }
      if (App.Instance.Inventory.GetItemCount(this.itemToGet) < 1) {
        Debug.Log($"No {this.itemToGet.Name} in Inventory");
        return ;
      }
      var item = App.Instance.Inventory.GetItem(this.itemToGet);
      Item.CreateItemObjectFrom(item.Data);
    }

    [Button ("Test print Inventory items")]
    void TestPrintInventory()
    {
      this.PrintInventory(App.Instance.Inventory);
    }

    [Button ("Move item to quickslot")]
    void TestMoveItemToQuickSlot()
    {
      if (this.itemToMoveQuickSlot == null) {
        Debug.Log($"not selected item to move to quickslot");
        return ;
      }
      if (App.Instance.Inventory.GetItemCount(this.itemToMoveQuickSlot) == 0) {
        Debug.Log($"{this.itemToMoveQuickSlot.Name} is not in Inventory");
        return ;
      }
      try {
        App.Instance.Inventory.MoveItemToQuickSlot(this.itemToMoveQuickSlot);
      }
      catch (Exception e) {
        Debug.LogError(e.Message);
      }
    }

    [Button ("Get item from quickslot")]
    void TestGetItemFromQuickSlot()
    {
      if (this.itemToGetFromQuickSlot == null) {
        Debug.Log($"not selected item to get from quickslot");
        return ;
      }
      if (App.Instance.Inventory.GetItemCouontInQuickSlot(
          this.itemToGetFromQuickSlot) == 0) {
        Debug.Log($"{this.itemToGetFromQuickSlot.Name} is not in quickslot");
        return ;
      }
      try {
        App.Instance.Inventory.MoveItemFromQuickSlot(this.itemToGetFromQuickSlot); 
      } catch (Exception e) {
        Debug.LogError(e.Message);
      }
    }

    void PrintInventory(ItemStorageBase inventory)
    {
     #if UNITY_EDITOR
      Debug.Log("Inventory items");
      this.inventorySlot = $"{inventory.CountUsedSlot()} / {App.Instance.Inventory.MAX_SLOT_COUNT}";
      for (int i = 0; i < App.Instance.Inventory.ItemNamesForDebugging.Count; i++) {
        var itemName = App.Instance.Inventory.ItemNamesForDebugging[i];
        Debug.Log($"{i + 1}: {itemName}"); 
      }
      Debug.Log("Quick slot items");
      var quickslotItems = App.Instance.Inventory.ItemNamesForDebuggingInQuickSlot;
      for (int i = 0; i < quickslotItems.Count; i++) {
        Debug.Log($"{i + 1}: {quickslotItems[i]}"); 
      }
      #endif
    }

    // Start is called before the first frame update
    void OnEnable()
    {
      App.Instance.Inventory.OnChanged += this.PrintInventory;
    }

    void OnDisable()
    {
      if (App.Instance?.Inventory != null) {
        App.Instance.Inventory.OnChanged -= this.PrintInventory;
      }
    }
  }
}

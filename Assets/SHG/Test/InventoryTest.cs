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

    [Button ("Test add item")]
    void AddItemTest()
    {
      if (this.itemToAdd == null) {
        Debug.Log("itemToAdd is none");
        return ;
      }
      var item = Item.CreateItemFrom(this.itemToAdd);
      Inventory.Instance.AddItem(item);
    }

    [Button ("Test get item")]
    void GetItemTest()
    {
      if (this.itemToGet == null) {
        Debug.Log("itemToGet is none");
        return ;
      }
      if (Inventory.Instance.GetItemCount(this.itemToGet) < 1) {
        Debug.Log($"No {this.itemToGet.Name} in Inventory");
        return ;
      }
      var item = Inventory.Instance.GetItem(this.itemToGet);
      Item.CreateItemObjectFrom(item.Data);
    }

    [Button ("Test print Inventory items")]
    void TestPrintInventory()
    {
      this.PrintInventory(Inventory.Instance);
    }

    [Button ("Move item to quickslot")]
    void TestMoveItemToQuickSlot()
    {
      if (this.itemToMoveQuickSlot == null) {
        Debug.Log($"not selected item to move to quickslot");
        return ;
      }
      if (Inventory.Instance.GetItemCount(this.itemToMoveQuickSlot) == 0) {
        Debug.Log($"{this.itemToMoveQuickSlot.Name} is not in Inventory");
        return ;
      }
      try {
        Inventory.Instance.MoveItemToQuickSlot(this.itemToMoveQuickSlot);
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
      if (Inventory.Instance.GetItemCouontInQuickSlot(
          this.itemToGetFromQuickSlot) == 0) {
        Debug.Log($"{this.itemToGetFromQuickSlot.Name} is not in quickslot");
        return ;
      }
      try {
        Inventory.Instance.MoveItemFromQuickSlot(this.itemToGetFromQuickSlot); 
      } catch (Exception e) {
        Debug.LogError(e.Message);
      }
    }

    void PrintInventory(Inventory inventory)
    {
      Debug.Log("Inventory items");
     #if UNITY_EDITOR
      for (int i = 0; i < Inventory.Instance.ItemNamesForDebugging.Count; i++) {
        var itemName = Inventory.Instance.ItemNamesForDebugging[i];
        Debug.Log($"{i + 1}: {itemName}"); 
      }
      Debug.Log("Quick slot items");
      var quickslotItems = Inventory.Instance.ItemNamesForDebuggingInQuickSlot;
      for (int i = 0; i < quickslotItems.Count; i++) {
        Debug.Log($"{i + 1}: {quickslotItems[i]}"); 
      }
      #endif
    }

    // Start is called before the first frame update
    void OnEnable()
    {
      Inventory.Instance.OnChanged += this.PrintInventory;
    }

    void OnDisable()
    {
      if (Inventory.Instance != null) {
        Inventory.Instance.OnChanged -= this.PrintInventory;
      }
    }
  }
}

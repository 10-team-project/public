using System;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class ItemStorageTest : MonoBehaviour
  {
    [SerializeField]
    ItemData itemToAdd;
    [SerializeField]
    ItemData itemToGet;
    [SerializeField, ReadOnly]
    string itemStorageSlot;

    [Button ("Test add item")]
    void AddItemTest()
    {
      if (this.itemToAdd == null) {
        Debug.Log("itemToAdd is none");
        return ;
      }
      var item = Item.CreateItemFrom(this.itemToAdd);
      App.Instance.ItemStorage.AddItem(item);
    }

    [Button ("Test get item")]
    void GetItemTest()
    {
      if (this.itemToGet == null) {
        Debug.Log("itemToGet is none");
        return ;
      }
      if (App.Instance.ItemStorage.GetItemCount(this.itemToGet) < 1) {
        Debug.Log($"No {this.itemToGet.Name} in Inventory");
        return ;
      }
      var item = App.Instance.ItemStorage.GetItem(this.itemToGet);
      Item.CreateItemObjectFrom(item.Data);
    }

    [Button ("Test print storage items")]
    void TestPrintStorage()
    {
      this.PrintStorage(App.Instance.ItemStorage);
    }

    void PrintStorage(ItemStorageBase storage)
    {
     #if UNITY_EDITOR
      Debug.Log("Item storage items");
      this.itemStorageSlot = $"{storage.CountUsedSlot()} / {App.Instance.ItemStorage.MAX_SLOT_COUNT}";
      for (int i = 0; i < App.Instance.ItemStorage.ItemNamesForDebugging.Count; i++) {
        var itemName = App.Instance.ItemStorage.ItemNamesForDebugging[i];
        Debug.Log($"{i + 1}: {itemName}"); 
      }
      #endif
    }

    // Start is called before the first frame update
    void OnEnable()
    {
      App.Instance.ItemStorage.OnChanged += this.PrintStorage;
    }

    void OnDisable()
    {
      if (App.Instance?.ItemStorage != null) {
        App.Instance.ItemStorage.OnChanged -= this.PrintStorage;
      }
    }
  }
}

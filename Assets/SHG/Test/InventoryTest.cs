using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class InventoryTest : MonoBehaviour
  {
    // Start is called before the first frame update
    void Start()
    {
      Inventory.Instance.OnChange += this.PrintInventory;
    }

    void PrintInventory(Inventory inventory)
    {
      Debug.Log("Inventory changed");
      foreach (var name in inventory.ItemNamesForDebugging) {
        Debug.Log(name); 
      }
    }

    [Button ("Add item")]
    void AddItem(ItemData itemData)
    {
      Inventory.Instance.AddItem(itemData);
    }

    [Button ("Get Item")]
    void GetItem(ItemData itemData)
    {
      if (Inventory.Instance.GetItemCount(itemData) < 1) {
        Debug.Log($"No item for ");
        return ;
      }
      var item = Inventory.Instance.GetItem(itemData);
      Instantiate(item.Data.Prefab);
    }
  }
}

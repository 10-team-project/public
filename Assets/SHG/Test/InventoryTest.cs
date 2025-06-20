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

    [Button ("Test add item")]
    void AddItemTest()
    {
      if (this.itemToAdd == null) {
        Debug.Log("itemToAdd is none");
        return ;
      }
      Inventory.Instance.AddItem(this.itemToAdd);
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
      var gameObject = Instantiate(item.Data.Prefab);
    }

    [Button ("Test print Inventory items")]
    void TestPrintInventory()
    {
      this.PrintInventory(Inventory.Instance);
    }

    void PrintInventory(Inventory inventory)
    {
      Debug.Log("Inventory items");
      for (int i = 0; i < Inventory.Instance.ItemNamesForDebugging.Count; i++) {
        var itemName = Inventory.Instance.ItemNamesForDebugging[i];
        Debug.Log($"{i + 1}: {itemName}"); 
      }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
      Inventory.Instance.OnChange += this.PrintInventory;
    }

    void OnDisable()
    {
      Inventory.Instance.OnChange -= this.PrintInventory;
    }
  }
}

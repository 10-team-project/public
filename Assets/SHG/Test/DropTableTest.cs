using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class DropTableTest : MonoBehaviour
  {
    [Button ("New drop items")]
    void PrintDropNewDropItems()
    {
      Debug.Log("New drop items");
      var items = App.Instance.DropTable.GetAddedItems();
      foreach (var item in items) {
        Debug.Log(item.Name); 
      }
    }

    [Button ("Removed drop items")]
    void PrintRemovedDropItems()
    {
      Debug.Log("Removed drop items");
      var items = App.Instance.DropTable.GetRemovedItems();
      foreach (var item in items) {
        Debug.Log(item.Name); 
      }
    }
  }
}

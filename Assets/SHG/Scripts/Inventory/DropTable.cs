using System;
using System.Collections.Generic;
using UnityEngine;

namespace SHG
{
  public class DropTable : MonoBehaviour
  {
    HashSet<ItemData> additionalItems;
    HashSet<ItemData> removedItems;

    public List<ItemData> GetAdditionalItems() => new (this.additionalItems);
    public List<ItemData> GetRemovedItems() => new (this.removedItems);
    
    public DropTable()
    {

    }

    public void RegisterInventoryEvent(Inventory inventory)
    {
      inventory.OnDropChangeItemUsed += this.OnUseDropChangeItem;
    }

    void OnUseDropChangeItem(DropChangeItem item)
    {
      if (item.OnUse != null) {
        foreach (var changeOnUse in item.OnUse) {
          switch (changeOnUse.TableChange)
          {
            case ItemDropChange.DropTableChange.AddToTable:
              break;
            case ItemDropChange.DropTableChange.RemoveFromTable:
              break;
            default:
              throw (new NotImplementedException());
          }
        }
      }
    }
  }
}

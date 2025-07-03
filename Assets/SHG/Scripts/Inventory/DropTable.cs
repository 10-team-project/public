using System;
using System.Collections.Generic;

namespace SHG
{
  public class DropTable 
  {
    HashSet<ItemData> addedItems;
    HashSet<ItemData> removedItems;

    public List<ItemData> GetAddedItems() => new (this.addedItems);
    public List<ItemData> GetRemovedItems() => new (this.removedItems);
    
    public DropTable()
    {
      this.addedItems = new ();
      this.removedItems = new ();
    }

    public void RegisterInventoryEvent(Inventory inventory)
    {
      inventory.OnDropChangeItemUsed += this.OnUseDropChangeItem;
      inventory.OnObtainItem += this.OnObtainItem;
    }

    void OnUseDropChangeItem(DropChangeItem item)
    {
      if (item.OnUse != null) {
        foreach (var changeOnUse in item.OnUse) {
          this.ApplyItemChange(changeOnUse);
        }
      }
    }

    void OnObtainItem(ItemData item)
    {
      if (item is DropChangeItemData dropChangeItem &&
        dropChangeItem.OnObtain != null) {
        foreach (var changeOnObtain in dropChangeItem.OnObtain) {
          this.ApplyItemChange(changeOnObtain);
        }
      }
    }

    void ApplyItemChange(ItemDropChange change)
    {
      switch (change.TableChange)
      {
        case ItemDropChange.DropTableChange.AddToTable:
          this.addedItems.Add(change.Item);
          this.removedItems.Remove(change.Item);
          break;
        case ItemDropChange.DropTableChange.RemoveFromTable:
          this.addedItems.Remove(change.Item);
          this.removedItems.Add(change.Item);
          break;
        default:
          throw (new NotImplementedException());
      }

    }
  }
}

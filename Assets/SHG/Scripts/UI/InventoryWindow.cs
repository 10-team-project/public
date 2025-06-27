using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SHG
{
  public class InventoryWindow : ItemConatinerWindow
  {
    const MouseButton DRAG_BUTTON = ItemConatinerWindow.MouseButton.Left;
    const MouseButton USE_BUTTON = ItemConatinerWindow.MouseButton.Right;
    Func<ItemData, bool> filterItem;
    Label label;

    public InventoryWindow(Func<ItemData, bool> filterItem, ItemBox floatingItemBox): base (floatingItemBox, App.Instance.Inventory)
    {
      this.filterItem = filterItem;
    }

    protected override void CreateUI()
    {
      this.label = new Label();
      this.label.AddToClassList("window-label");
      this.Add(this.label);
    }

    //TODO: 각 아이템 UI를 objectpool에 보관
    ItemBox CreateItembox(ItemAndCount itemAndCount)
    {
      ItemBox itemBox = new ItemBox(this);
      itemBox.SetData(itemAndCount);
      itemBox.RegisterCallback<PointerDownEvent>(this.OnPointerDown);         
      itemBox.RegisterCallback<PointerUpEvent>(this.OnPointerUp);
      itemBox.RegisterCallback<PointerMoveEvent>(this.OnPointerMove);
      return (itemBox);
    }

    protected override void FillItems(ItemStorageBase inventory)
    {
      foreach (var pair in inventory.Items) {
        var (item, count) = pair;
        if (count > 0 && this.filterItem(item)) {
          var box = this.CreateItembox(
            new ItemAndCount { Item = item, Count = count });
          this.itemsContainer.Add(box);
        }
      } 
    }

    // TODO: return to Obejctpool
    protected override void ClearItems()
    {
      this.itemsContainer.Clear();
    }

    protected override void OnUsePointerButtonDown(ItemBox boxElement, ItemAndCount itemAndCount)
    {
      var item = App.Instance.Inventory.PeakItem(itemAndCount.Item); 
      if (item is IUsable usableItem) {
        this.UseItem(App.Instance.Inventory.GetItem(itemAndCount.Item) as IUsable);
      }
    }

    void UseItem(IUsable usableItem)
    {
      App.Instance.Inventory.UseItem(usableItem);
    }

    protected override bool IsAbleToDropItem(ItemData item, ItemConatinerWindow targetContainer)
    {
      if (targetContainer is QuickSlotWindow quickSlotWindow) {
        return (item as EquipmentItemData);
      }
      else if (targetContainer is ItemStorageWindow itemStorageWindow) {
        return (true);
      }
      return (false);
    }

    protected override void DropItem(ItemAndCount itemAndCount, ItemConatinerWindow targetContainer)
    {
      if (targetContainer is QuickSlotWindow) {
        App.Instance.Inventory.MoveItemToQuickSlot(itemAndCount.Item);
      }
      else if (targetContainer is ItemStorageWindow itemStorageWindow) {
        if (itemAndCount.Count == 1) {
          var item = this.ItemSource.GetItem(itemAndCount.Item);
          targetContainer.ItemSource.AddItem(item);
        } 
        else {
          var (item, count) = this.ItemSource.GetItems(itemAndCount.Item, itemAndCount.Count);
          targetContainer.ItemSource.AddItems(item, count);
        }
      }
      else {
        throw (new NotImplementedException());
      }
    }

    protected override void DropItemOutSide(ItemAndCount itemAndCount)
    {
      //FIXME: drop item count?
      App.Instance.Inventory.GetItem(itemAndCount.Item);
    }

    protected override bool IsAbleToDropOutSide(ItemData item)
    {
      return (true);
    }
  }
}

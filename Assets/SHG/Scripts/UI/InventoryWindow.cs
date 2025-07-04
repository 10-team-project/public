using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class InventoryWindow : ItemStorageWindow
  {
    const MouseButton DRAG_BUTTON = ItemStorageWindow.MouseButton.Left;
    const MouseButton USE_BUTTON = ItemStorageWindow.MouseButton.Right;
    Func<ItemData, bool> filterItem;
    Label label;
    protected override Vector2 DescriptionOffset => DESCRIPTION_OFFSET;
    readonly Vector2 DESCRIPTION_OFFSET = new Vector2(0f, 150f);

    public InventoryWindow(Func<ItemData, bool> filterItem, ItemBox floatingItemBox): base (floatingItemBox, App.Instance.Inventory)
    {
      this.filterItem = filterItem;
    }

    protected override void CreateUI()
    {
      this.label = new Label();
      Utils.HideVisualElement(this.itemDescriptionContainer);
    }

    //TODO: 각 아이템 UI를 objectpool에 보관
    ItemBox CreateItembox(ItemAndCount itemAndCount)
    {
      ItemBox itemBox = new ItemBox(this);
      itemBox.SetData(itemAndCount);
      itemBox.RegisterCallback<PointerDownEvent>(this.OnPointerDown);         
      itemBox.RegisterCallback<PointerUpEvent>(this.OnPointerUp);
      itemBox.RegisterCallback<PointerMoveEvent>(this.OnPointerMove);
      itemBox.RegisterCallback<PointerOverEvent>(this.OnPointerOver);
      itemBox.RegisterCallback<PointerLeaveEvent>(this.OnPointerLeave);
      return (itemBox);
    }

    void UseItem(IUsable usableItem)
    {
      App.Instance.Inventory.UseItem(usableItem);
    }

    protected override void FillItems(ItemStorageBase inventory)
    {
      foreach (var pair in inventory.Items) {
        var (item, count) = pair;
        if (!this.filterItem(item) || count < 1) {
          continue;
        }
        if (item.IsStoryItem) {
          for (int i = 0; i < count; i++) {
          var box = this.CreateItembox(
            new ItemAndCount { Item = item, Count = 1 });
          this.itemsContainer.Add(box);
          } 
        }
        else {
          var stackCount = count / inventory.MAX_STACK_COUNT; 
          var restCount = count % inventory.MAX_STACK_COUNT;
          for (int i = 0; i < stackCount; i++) {
            var box = this.CreateItembox(
              new ItemAndCount { 
              Item = item, Count = inventory.MAX_STACK_COUNT });
            this.itemsContainer.Add(box);
             
          }
          if (restCount != 0) {
            var box = this.CreateItembox(
              new ItemAndCount { 
              Item = item, Count = restCount });
            this.itemsContainer.Add(box);
          }
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

    protected override bool IsAbleToDropItem(ItemData item, ItemStorageWindow targetContainer)
    {
      if (targetContainer is QuickSlotWindow quickSlotWindow) {
        return (item as EquipmentItemData);
      }
      else if (targetContainer is ItemLockerWindow itemStorageWindow) {
        return (true);
      }
      return (false);
    }

    protected override void DropItem(ItemAndCount itemAndCount, ItemStorageWindow targetContainer)
    {
      if (targetContainer is QuickSlotWindow) {
        App.Instance.Inventory.MoveItemToQuickSlot(itemAndCount.Item);
      }
      else if (targetContainer is ItemLockerWindow itemStorageWindow) {
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

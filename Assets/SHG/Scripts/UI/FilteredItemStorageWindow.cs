using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SHG
{
  public class FilteredItemStorageWindow : ItemStorageWindow
  {
    const MouseButton DRAG_BUTTON = ItemStorageWindow.MouseButton.Left;
    const MouseButton USE_BUTTON = ItemStorageWindow.MouseButton.Right;
    Dictionary<VisualElement, ItemAndCount> itemBoxTable;
    Func<ItemData, bool> filterItem;

    public FilteredItemStorageWindow(Func<ItemData, bool> filterItem, ItemBox floatingItemBox): base (floatingItemBox)
    {
      this.itemBoxTable = new ();
      this.filterItem = filterItem;
    }

    protected override void CreateUI()
    {
      var label = new Label();
      label.text = this.label;
      label.AddToClassList("window-label");
      this.Add(label);
    }

    //TODO: 각 아이템 UI를 objectpool에 보관
    ItemBox CreateItembox(ItemAndCount itemAndCount)
    {
      ItemBox itemBox = new ItemBox(this);
      itemBox.SetData(itemAndCount);
      itemBox.RegisterCallback<PointerDownEvent>(this.OnPointerDown);         
      itemBox.RegisterCallback<PointerUpEvent>(this.OnPointerUp);
      itemBox.RegisterCallback<PointerMoveEvent>(this.OnPointerMove);
      this.itemBoxTable[itemBox] = itemAndCount;
      return (itemBox);
    }

    protected override void FillItems(Inventory inventory)
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
      var item = Inventory.Instance.PeakItem(itemAndCount.Item); 
      if (item is IUsable usableItem) {
        this.UseItem(Inventory.Instance.GetItem(itemAndCount.Item) as IUsable);
      }
    }

    void UseItem(IUsable usableItem)
    {
      Inventory.Instance.UseItem(usableItem);
    }

    void OnDropItemToQuickSlot(EquipmentItemData equipmentItemData)
    {
      Inventory.Instance.MoveItemToQuickSlot(equipmentItemData);
    }

    protected override bool IsAbleToDropItem(ItemData item)
    {
      return (item as EquipmentItemData);
    }

    protected override void DropItem(ItemAndCount itemAndCount)
    {
      Inventory.Instance.MoveItemToQuickSlot(itemAndCount.Item);
    }

    protected override void DropItemOutSide(ItemAndCount itemAndCount)
    {
      //FIXME: drop item count?
      Inventory.Instance.GetItem(itemAndCount.Item);
    }

    protected override bool IsAbleToDropOutSide(ItemData item)
    {
      return (true);
    }
  }
}

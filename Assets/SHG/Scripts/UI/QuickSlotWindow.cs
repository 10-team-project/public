using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class QuickSlotWindow : ItemConatinerWindow
  {
    ItemBox[] slots;
    Dictionary<VisualElement, ItemAndCount> itemBoxTable;
    ItemConatinerWindow[] dropTargets; 

    public QuickSlotWindow(ItemBox floatingItemBox): base (floatingItemBox)
    {
      this.name = "quick-slot-window-container";
      this.Show();
    }

    protected override void CreateUI()
    {
      this.slots = new ItemBox[Inventory.QUICKSLOT_COUNT];
      for (int i = 0; i < Inventory.QUICKSLOT_COUNT; i++)
      {
        this.slots[i]= new ItemBox(this);
        this.slots[i].RegisterCallback<PointerDownEvent>(this.OnPointerDown);         
        this.slots[i].RegisterCallback<PointerUpEvent>(this.OnPointerUp);
        this.slots[i].RegisterCallback<PointerMoveEvent>(this.OnPointerMove);
        this.slots[i].AddToClassList("quick-slot-window-item-box");
        this.itemsContainer.Add(this.slots[i]); 
      }
    }

    protected override void ClearItems()
    {
      for (int i = 0; i < Inventory.QUICKSLOT_COUNT; i++) {
        this.slots[i].RemoveData();  
      }
    }

    protected override void FillItems(ItemStorageBase inventoryBase)
    {
      for (int i = 0; i < App.Instance.Inventory.QuickSlotItems.Count; i++) {
        this.slots[i].SetData(new ItemAndCount { Item = App.Instance.Inventory.QuickSlotItems[i], Count = 1 }) ;
      }
    }

    protected override void OnUsePointerButtonDown(ItemBox boxElement, ItemAndCount itemAndCount)
    {
      return ;
    }

    protected override bool IsAbleToDropItem(ItemData item)
    {
      return (item as EquipmentItemData);
    }

    protected override void DropItem(ItemAndCount itemAndCount)
    {
      App.Instance.Inventory.MoveItemFromQuickSlot(itemAndCount.Item);
    }

    protected override void DropItemOutSide(ItemAndCount itemAndCount)
    {
    }

    protected override bool IsAbleToDropOutSide(ItemData item)
    {
      return (false);
    }
  }
}

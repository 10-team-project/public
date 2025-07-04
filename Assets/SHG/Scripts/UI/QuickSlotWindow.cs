using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class QuickSlotWindow : ItemStorageWindow
  {
    ItemBox[] slots;
    Dictionary<VisualElement, ItemAndCount> itemBoxTable;
    ItemStorageWindow[] dropTargets; 

    public QuickSlotWindow(ItemBox floatingItemBox): base (floatingItemBox, App.Instance.Inventory)
    {
      this.name = "quick-slot-window-container";
      this.RemoveFromClassList("item-storage-container");
      this.Show();
    }
    
    public bool TryGetQuickslotItem(int slotNumber, out EquipmentItemData item)
    {
      var itemInSlot = this.slots[slotNumber].ItemData;
      if (itemInSlot != ItemAndCount.None) {
        item = itemInSlot.Item as EquipmentItemData;
        return (true);
      }  
      item = null;
      return (false);
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

    protected override bool IsAbleToDropItem(ItemData item, ItemStorageWindow targetContainer)
    {
      if (targetContainer is InventoryWindow InventoryItemContainerWindow) {
        return (item as EquipmentItemData);
      }
      return (false);
    }

    protected override void DropItem(ItemAndCount itemAndCount, ItemStorageWindow targetContainer)
    {
      if (targetContainer is InventoryWindow InventoryItemContainerWindow) {
        App.Instance.Inventory.MoveItemFromQuickSlot(itemAndCount.Item);
      }
      else {

      }
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

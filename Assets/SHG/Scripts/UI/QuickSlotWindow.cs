using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class QuickSlotWindow : VisualElement, IHideableWindow
  {
    public bool IsVisiable { get; private set; }
    public bool IsDraggingItem => this.currentDraggingTarget != null;

    ItemBox[] slots;
    Vector2 dragStartPosition;
    ItemBox currentDraggingTarget;
    ItemBox floatingItemBox;

    public QuickSlotWindow(ItemBox floatingItemBox)
    {
      this.name = "quick-slot-window";     
      this.floatingItemBox = floatingItemBox;
      this.AddToClassList("window-container");
      this.CreateUI();
      Inventory.Instance.OnChanged += this.OnChangeInventory;
      this.OnChangeInventory(Inventory.Instance);
    }

    void OnChangeInventory(Inventory inventory)
    {
      var itemCount = inventory.QuickSlotItems.Count;
      for (int i = 0; i < this.slots.Length; i++) {
        this.slots[i].RemoveData();
      }
      for (int i = 0; i < itemCount; i++) {
        var item = inventory.QuickSlotItems[i];
        this.slots[i].SetData(new ItemAndCount { Item = item, Count = 1});
      }
    }

    void CreateUI()
    {
      this.slots = new ItemBox[Inventory.QUICKSLOT_COUNT];
      for (int i = 0; i < this.slots.Length; i++) {
        this.slots[i] = new ItemBox(this);
        this.slots[i].RegisterCallback<PointerDownEvent>(this.OnPointerDown);
        this.slots[i].RegisterCallback<PointerMoveEvent>(this.OnPointerMove);
        this.slots[i].RegisterCallback<PointerUpEvent>(this.OnPointerUp);
        this.Add(this.slots[i]);
      }
    }

    void OnPointerDown(PointerDownEvent pointerDownEvent)
    {
      if (!this.IsDraggingItem) {
        var boxElement = ItemBox.FindItemBoxFrom(pointerDownEvent.target as VisualElement);
        this.dragStartPosition = pointerDownEvent.position;
        if (boxElement != null &&
          boxElement.ItemData != ItemAndCount.None) {
          this.floatingItemBox.SetData(boxElement.ItemData);
          this.floatingItemBox.style.left = this.dragStartPosition.x;
          this.floatingItemBox.style.top = this.dragStartPosition.y;
          this.floatingItemBox.Show();
          boxElement.AddToClassList("inventory-item-box-inactive");
          this.currentDraggingTarget = boxElement;
          boxElement.CapturePointer(pointerDownEvent.pointerId);
        }
        else {
          Debug.LogError($"Pointer target is not in ItemBox or itemBoxTable");
        }
      }
      else {
        Debug.Log($"Already dragging item");
      }
    }

    ItemBox FindSelectedItemBox(VisualElement toFound)
    {
      return (Array.Find(this.slots, slot => slot == toFound));
    }

    void OnPointerMove(PointerMoveEvent pointerMoveEvent)
    {
      if (this.IsDraggingItem) {
        var offset = new Vector2(
          pointerMoveEvent.position.x,
          pointerMoveEvent.position.y) - this.dragStartPosition;
        this.floatingItemBox.UpdateOffset(offset); 
      }
    }

    void OnPointerUp(PointerUpEvent pointerUpEvent)
    {
      if (this.IsDraggingItem &&
        pointerUpEvent.target == this.currentDraggingTarget) {
        this.currentDraggingTarget.RemoveFromClassList("inventory-item-box-inactive");
        this.floatingItemBox.UpdateOffset(Vector2.zero);
        this.floatingItemBox.Hide();
        VisualElement target = this.panel.Pick(pointerUpEvent.position);
        bool isDroppingToInventory = this.IsDroppingToInventory(target);
        if (isDroppingToInventory &&
          this.currentDraggingTarget.ItemData.Item is EquipmentItemData equipmentItemData) {
          this.OnDropItemToInventory(equipmentItemData); 
        }
        this.currentDraggingTarget.ReleasePointer(pointerUpEvent.pointerId);
        this.currentDraggingTarget = null;
        this.dragStartPosition = Vector2.zero;
      }
    }

    void OnDropItemToInventory(EquipmentItemData equipmentItemData)
    {
      Debug.Log("OnDropItemToInventory");
      Inventory.Instance.MoveItemFromQuickSlot(equipmentItemData);
    }

    bool IsDroppingToInventory(VisualElement target)
    {
      if (target is InventoryWindow) {
        return (true);
      }
      ItemBox destBox = ItemBox.FindItemBoxFrom(target);
      if (destBox != null &&
        destBox.ParentWindow is InventoryWindow) {
        return (true);
      }
      return (false);
    }

    public void Show()
    {
      this.IsVisiable = true;
      this.style.display = DisplayStyle.Flex;
      this.BringToFront();
    }

    public void Hide()
    {
      this.IsVisiable = false;
      this.style.display = DisplayStyle.None;
      this.SendToBack();
    }
  }
}

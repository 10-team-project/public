using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  enum MouseButton
  {
    Left = 0,
    Right = 1
  }
  public class InventoryWindow : VisualElement, IHideableWindow
  {
    const MouseButton DRAG_BUTTON = MouseButton.Left;
    const MouseButton USE_BUTTON = MouseButton.Right;
    public bool IsVisiable { get; private set; }
    VisualElement itemsContainer;
    VisualElement currentDraggingTarget;
    bool IsDraggingItem => this.currentDraggingTarget != null;
    Dictionary<VisualElement, ItemAndCount> itemBoxTable;
    Vector2 dragStartPosition;
    ItemBox floatingItemBox;

    public InventoryWindow(ItemBox floatingItemBox)
    {
      this.name = "inventory-window-container";
      this.itemBoxTable = new ();
      this.floatingItemBox = floatingItemBox;
      this.AddToClassList("window-container");
      this.CreateUI();
    }

    public void Show()
    {
      Inventory.Instance.OnChanged += this.OnInventoryUpdated;
      this.OnInventoryUpdated(Inventory.Instance);
      this.IsVisiable = true;
      this.style.display = DisplayStyle.Flex;
      this.BringToFront();
    }

    public void Hide()
    {
      Inventory.Instance.OnChanged -= this.OnInventoryUpdated;
      this.IsVisiable = false;
      this.style.display = DisplayStyle.None;
      this.SendToBack();
    }

    public void OnInventoryUpdated(Inventory inventory)
    {
      this.ClearInventoryItems();
      this.FillInventoryItems(inventory);
    }

    void CreateUI()
    {
      var label = new Label();
      label.text = "Inventory";
      label.AddToClassList("window-label");
      this.Add(label);
      this.itemsContainer = new VisualElement();
      this.itemsContainer.name = "inventory-window-items-container";
      this.Add(this.itemsContainer);
      var closeButton = new Button();
      closeButton.text = "close";
      closeButton.RegisterCallback<ClickEvent>(click => this.Hide());
      closeButton.AddToClassList("window-close-button"); 
      this.Add(closeButton);
    }

    void FillInventoryItems(Inventory inventory)
    {
      foreach (var pair in inventory.Items) {
        var (item, count) = pair;
        if (count > 0) {
          var box = this.CreateItembox(
            new ItemAndCount { Item = item, Count = count });
          this.itemsContainer.Add(box);
        }
      } 
    }

    // TODO: return to Obejctpool
    void ClearInventoryItems()
    {
      this.itemsContainer.Clear();
    }

    //TODO: 각 아이템 UI를 objectpool에 보관
    VisualElement CreateItembox(ItemAndCount itemAndCount)
    {
      ItemBox itemBox = new ItemBox(this);
      itemBox.SetData(itemAndCount);
      itemBox.RegisterCallback<PointerDownEvent>(this.OnPointerDown);         
      itemBox.RegisterCallback<PointerUpEvent>(this.OnPointerUp);
      itemBox.RegisterCallback<PointerMoveEvent>(this.OnPointerMove);
      this.itemBoxTable[itemBox] = itemAndCount;
      return (itemBox);
    }

    void OnPointerDown(PointerDownEvent pointerDownEvent)
    {
      var boxElement = ItemBox.FindItemBoxFrom(pointerDownEvent.target as VisualElement);
      if (boxElement == null ||
        !this.itemBoxTable.TryGetValue(boxElement,
          out ItemAndCount itemAndCount)) {
        Debug.LogError($"Pointer target is not in ItemBox or itemBoxTable");
        return ;
      }
      if (pointerDownEvent.button == (int)DRAG_BUTTON) {
        this.OnDragPointerButtonDown(pointerDownEvent, boxElement, itemAndCount);
      }
      else {
        this.OnUsePointerButtonDown(boxElement, itemAndCount); 
      }
    }

    void OnUsePointerButtonDown(ItemBox boxElement, ItemAndCount itemAndCount)
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

    void OnDragPointerButtonDown(PointerDownEvent pointerDownEvent, ItemBox boxElement, ItemAndCount itemAndCount)
    {
      if (!this.IsDraggingItem) {
        this.dragStartPosition = pointerDownEvent.position;
        boxElement.AddToClassList("inventory-item-box-inactive");
        this.floatingItemBox.SetData(boxElement.ItemData);
        this.floatingItemBox.style.left = this.dragStartPosition.x;
        this.floatingItemBox.style.top = this.dragStartPosition.y;
        this.floatingItemBox.Show();
        this.currentDraggingTarget = boxElement;
        boxElement.CapturePointer(pointerDownEvent.pointerId);
      }
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
      if (this.IsDraggingItem) {
        this.floatingItemBox.UpdateOffset(Vector2.zero);
        this.currentDraggingTarget.RemoveFromClassList("inventory-item-box-inactive");
        this.floatingItemBox.Hide();
        VisualElement target = this.panel.Pick(pointerUpEvent.position);
        bool isDroppingToQuickSlot = this.IsDroppingToQuickSlot(target);
        if (isDroppingToQuickSlot && 
          this.itemBoxTable.TryGetValue(this.currentDraggingTarget, out ItemAndCount itemAndCount) &&
          itemAndCount.Item is EquipmentItemData equipmentItemData) {
          this.OnDropItemToQuickSlot(equipmentItemData); 
        }
        this.currentDraggingTarget.ReleasePointer(pointerUpEvent.pointerId);
        this.currentDraggingTarget = null;
        this.dragStartPosition = Vector2.zero;
      }
    }

    bool IsDroppingToQuickSlot(VisualElement target)
    {
      if (target is QuickSlotWindow quickSlotWindow) {
        return (true);
      }
      else {
        ItemBox foundBox = ItemBox.FindItemBoxFrom(target);
        if (foundBox != null &&
          foundBox.ParentWindow is QuickSlotWindow) {
          return (true);
        } 
      }
      return (false);
    }

    void OnDropItemToQuickSlot(EquipmentItemData equipmentItemData)
    {
      Inventory.Instance.MoveItemToQuickSlot(equipmentItemData);
    }
  }
}

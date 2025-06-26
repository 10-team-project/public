using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public abstract class ItemStorageWindow: VisualElement, IHideableWindow
  {
    const MouseButton DRAG_BUTTON = MouseButton.Left;
    const MouseButton USE_BUTTON = MouseButton.Right;
    public bool IsVisiable { get; protected set; }
    protected string label;
    protected VisualElement itemsContainer;
    protected VisualElement currentDraggingTarget;
    protected bool IsDraggingItem => this.currentDraggingTarget != null;
    protected Vector2 dragStartPosition;
    protected ItemBox floatingItemBox;
    protected abstract ItemStorageWindow[] DropTargets { get; }

    public ItemStorageWindow(ItemBox floatingItemBox)
    {
      this.floatingItemBox = floatingItemBox;
      this.AddToClassList("item-storage-container");
      this.AddToClassList("window-container");
      this.itemsContainer = new VisualElement();
      this.itemsContainer.AddToClassList("item-storage-items-container");
      this.Add(this.itemsContainer);
      this.CreateUI();
      this.OnInventoryUpdated(Inventory.Instance);
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

    protected void OnInventoryUpdated(Inventory inventory)
    {
      this.ClearItems();
      this.FillItems(inventory);
    }

    protected abstract void FillItems(Inventory inventory);

    protected abstract void OnUsePointerButtonDown(ItemBox boxElement, ItemAndCount itemAndCount);
    protected abstract bool IsAbleToDropItem(ItemData item);
    protected abstract void DropItem(ItemAndCount itemAndCount);
    protected abstract void DropItemOutSide(ItemAndCount itemAndCount);
    protected abstract bool IsAbleToDropOutSide(ItemData item);

    protected abstract void CreateUI();

    // TODO: return to Obejctpool
    protected abstract void ClearItems();

    protected void OnPointerDown(PointerDownEvent pointerDownEvent)
    {
      var boxElement = Utils.FindUIElementFrom<ItemBox>(pointerDownEvent.target as VisualElement);
      if (boxElement == null) {
        return ;
      }
      var itemAndCount = boxElement.ItemData;
      if (pointerDownEvent.button == (int)DRAG_BUTTON) {
        this.OnDragPointerButtonDown(pointerDownEvent, boxElement, itemAndCount);
      }
      else if (pointerDownEvent.button == (int)USE_BUTTON) {
        this.OnUsePointerButtonDown(boxElement, itemAndCount); 
      }
    }

    protected virtual void OnDragPointerButtonDown(PointerDownEvent pointerDownEvent, ItemBox boxElement, ItemAndCount itemAndCount)
    {
      if (!this.IsDraggingItem && boxElement.ItemData != null) {
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

    protected virtual void OnPointerMove(PointerMoveEvent pointerMoveEvent)
    {
      if (this.IsDraggingItem) {
        var offset = new Vector2(
          pointerMoveEvent.position.x,
          pointerMoveEvent.position.y) - this.dragStartPosition;
        this.floatingItemBox.UpdateOffset(offset); 
      } 
    }

    protected virtual void OnPointerUp(PointerUpEvent pointerUpEvent)
    {
      if (this.IsDraggingItem) {
        this.floatingItemBox.UpdateOffset(Vector2.zero);
        this.currentDraggingTarget.RemoveFromClassList("inventory-item-box-inactive");
        this.floatingItemBox.Hide();

        VisualElement target = this.panel.Pick(pointerUpEvent.position);
        if (this.floatingItemBox.ItemData != null && !this.Contains(target)) {
          bool isDropToTargetStorage = this.IsDropTargetStorage(target);
          var itemAndCount = this.floatingItemBox.ItemData;
          if (isDropToTargetStorage &&
            this.IsAbleToDropItem(itemAndCount.Item)) {
            this.DropItem(itemAndCount); 
          }
          else if (!isDropToTargetStorage && this.IsAbleToDropOutSide(itemAndCount.Item)) {
            this.DropItemOutSide(itemAndCount); 
          }
        }
        this.currentDraggingTarget.ReleasePointer(pointerUpEvent.pointerId);
        this.currentDraggingTarget = null;
        this.dragStartPosition = Vector2.zero;
      }
    }

    bool IsDropTarget(ItemStorageWindow window)
    {
      foreach (var dropTarget in this.DropTargets) {
        if (window == dropTarget) {
          return (true);
        }
      }
      return (false);
    }

    protected bool IsDropTargetStorage(VisualElement target)
    {
      ItemBox foundBox = Utils.FindUIElementFrom<ItemBox>(target);
      if (foundBox != null &&
        foundBox.ParentWindow is ItemStorageWindow parentWindow) {
        return (this.IsDropTarget(parentWindow));
      } 
      ItemStorageWindow storageWindow = Utils.FindUIElementFrom<ItemStorageWindow>(target);
      if (storageWindow != null) {
        return (this.IsDropTarget(storageWindow));
      }
      return (false);
    }
  }
}

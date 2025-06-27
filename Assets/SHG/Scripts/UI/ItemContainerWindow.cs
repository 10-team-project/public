using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public abstract class ItemConatinerWindow: VisualElement, IHideableUI
  {
    public enum MouseButton
    {
      Left = 0,
      Right = 1
    }
    const MouseButton DRAG_BUTTON = MouseButton.Left;
    const MouseButton USE_BUTTON = MouseButton.Right;
    public bool IsVisiable { get; protected set; }
    protected string label;
    protected VisualElement itemsContainer;
    protected VisualElement currentDraggingTarget;
    protected bool IsDraggingItem => this.currentDraggingTarget != null;
    protected Vector2 dragStartPosition;
    protected ItemBox floatingItemBox;
    public List<ItemConatinerWindow> DropTargets { get; protected set; }

    public ItemConatinerWindow(ItemBox floatingItemBox)
    {
      this.floatingItemBox = floatingItemBox;
      this.DropTargets = new ();
      this.AddToClassList("item-storage-container");
      this.AddToClassList("window-container");
      this.itemsContainer = new VisualElement();
      this.itemsContainer.AddToClassList("item-storage-items-container");
      this.Add(this.itemsContainer);
      this.CreateUI();
      this.OnInventoryUpdated(App.Instance.Inventory);
    }

    public void AddDropTargets(IEnumerable<ItemConatinerWindow> targets)
    {
      foreach (var target in targets) {
        if (target == this) {
          throw (new ArgumentException($"drop target same window {target}"));
        }
        this.DropTargets.Add(target);
      }
    }

    public void Show()
    {
      App.Instance.Inventory.OnChanged += this.OnInventoryUpdated;
      this.OnInventoryUpdated(App.Instance.Inventory);
      this.IsVisiable = true;
      Utils.ShowVisualElement(this);
    }

    public void Hide()
    {
      App.Instance.Inventory.OnChanged -= this.OnInventoryUpdated;
      this.IsVisiable = false;
      Utils.HideVisualElement(this);
    }

    protected void OnInventoryUpdated(ItemStorageBase inventory)
    {
      this.ClearItems();
      this.FillItems(inventory);
    }

    protected abstract void FillItems(ItemStorageBase inventory);

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
      if (!this.IsDraggingItem && boxElement.ItemData != ItemAndCount.None) {
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
        if (this != target && !this.Contains(target) &&
          this.floatingItemBox.ItemData != ItemAndCount.None) {

          bool isDropToTargetStorage = this.IsDropTargetStorage(target);
          var itemAndCount = this.floatingItemBox.ItemData;
          if (isDropToTargetStorage &&
            this.IsAbleToDropItem(itemAndCount.Item)) {
            this.DropItem(itemAndCount); 
          }
          else if (!isDropToTargetStorage && this.IsAbleToDropOutSide(itemAndCount.Item)) {
            Debug.Log($"target: {target}");
            this.DropItemOutSide(itemAndCount); 
          }

        }
        this.currentDraggingTarget.ReleasePointer(pointerUpEvent.pointerId);
        this.currentDraggingTarget = null;
        this.dragStartPosition = Vector2.zero;
      }
    }

    bool IsDropTarget(ItemConatinerWindow window)
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
        foundBox.ParentWindow is ItemConatinerWindow parentWindow) {
        return (this.IsDropTarget(parentWindow));
      } 
      ItemConatinerWindow storageWindow = Utils.FindUIElementFrom<ItemConatinerWindow>(target);
      if (storageWindow != null) {
        return (this.IsDropTarget(storageWindow));
      }
      return (false);
    }
  }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public abstract class ItemStorageWindow: VisualElement, IHideableUI
  {
    public enum MouseButton
    {
      Left = 0,
      Right = 1
    }
    const MouseButton DRAG_BUTTON = MouseButton.Left;
    const MouseButton USE_BUTTON = MouseButton.Right;
    public bool IsVisiable { get; protected set; }
    public List<ItemStorageWindow> DropTargets { get; protected set; }
    public ItemStorageBase ItemSource { get; protected set; }
    protected VisualElement itemsContainer;
    protected VisualElement currentDraggingTarget;
    protected bool IsDraggingItem => this.currentDraggingTarget != null;
    protected Vector2 dragStartPosition;
    protected ItemBox floatingItemBox;
    protected Label itemDescriptionTitle;
    protected Label itemDescriptionContent;
    protected VisualElement itemDescriptionContainer;
    protected virtual Vector2 DescriptionOffset => Vector2.zero;

    public ItemStorageWindow(ItemBox floatingItemBox, ItemStorageBase itemSource)
    {
      this.floatingItemBox = floatingItemBox;
      this.ItemSource = itemSource;
      this.DropTargets = new ();
      this.AddToClassList("item-storage-container");
      this.AddToClassList("window-container");
      this.itemsContainer = new VisualElement();
      this.itemsContainer.AddToClassList("item-storage-items-container");
      this.Add(this.itemsContainer);
      this.CreateUI();
      this.OnItemSourceUpdated(this.ItemSource);
    }

    public void AddDropTargets(IEnumerable<ItemStorageWindow> targets)
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
      this.ItemSource.OnChanged += this.OnItemSourceUpdated;
      this.OnItemSourceUpdated(this.ItemSource);
      this.IsVisiable = true;
      Utils.ShowVisualElement(this);
    }

    public void Hide()
    {
      this.ItemSource.OnChanged -= this.OnItemSourceUpdated;
      this.IsVisiable = false;
      Utils.HideVisualElement(this);
    }

    protected void OnItemSourceUpdated(ItemStorageBase itemSource)
    {
      this.ClearItems();
      this.FillItems(itemSource);
    }

    protected abstract void FillItems(ItemStorageBase inventory);

    protected abstract void OnUsePointerButtonDown(ItemBox boxElement, ItemAndCount itemAndCount);
    protected abstract bool IsAbleToDropItem(ItemData item, ItemStorageWindow targetContainer);
    protected abstract void DropItem(ItemAndCount itemAndCount, ItemStorageWindow targetContainer);
    protected abstract void DropItemOutSide(ItemAndCount itemAndCount);
    protected abstract bool IsAbleToDropOutSide(ItemData item);
    protected virtual void OnHoverItemBox(ItemBox boxElement, PointerOverEvent pointerOverEvent) {
      if (boxElement.ItemData != ItemAndCount.None) {
        this.itemDescriptionTitle.text = boxElement.ItemData.Item.Description;
        var pos = boxElement.localBound.position;
        this.itemDescriptionContainer.style.left = pos.x + this.DescriptionOffset.x;
         this.itemDescriptionContainer.style.top = pos.y + this.DescriptionOffset.y;
        Utils.ShowVisualElement(this.itemDescriptionContainer);
      }
    }

    protected virtual void OnLeaveItemBox(ItemBox boxElement, PointerLeaveEvent pointerLeaveEvent) { 
      if (boxElement.ItemData != ItemAndCount.None)
      {
        Utils.HideVisualElement(this.itemDescriptionContainer);
      }
    }

    protected abstract void CreateUI();

    // TODO: return to Obejctpool
    protected abstract void ClearItems();


    protected virtual void OnPointerOver(PointerOverEvent pointerOverEvent)
    {
      var boxElement = Utils.FindUIElementFrom<ItemBox>(pointerOverEvent.target as VisualElement);
      if (boxElement != null) {
        this.OnHoverItemBox(boxElement, pointerOverEvent);
      }
    }

    protected virtual void OnPointerLeave(PointerLeaveEvent pointerLeaveEvent)
    {
      var boxElement = Utils.FindUIElementFrom<ItemBox>(pointerLeaveEvent.target as VisualElement);
      if (boxElement != null) {
        this.OnLeaveItemBox(boxElement, pointerLeaveEvent);
      }
    }
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

          bool isDropToTargetStorage = this.IsDropTargetStorage(target, out ItemStorageWindow targetContainer);
          var itemAndCount = this.floatingItemBox.ItemData;
          if (isDropToTargetStorage &&
            this.IsAbleToDropItem(itemAndCount.Item, targetContainer)) {
            this.DropItem(itemAndCount, targetContainer); 
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

    protected bool IsDropTargetStorage(VisualElement target, out ItemStorageWindow targetContainer)
    {
      ItemBox foundBox = Utils.FindUIElementFrom<ItemBox>(target);
      if (foundBox != null &&
        foundBox.ParentWindow is ItemStorageWindow parentWindow) {
        targetContainer = parentWindow;
        return (this.IsDropTarget(parentWindow));
      } 
      ItemStorageWindow storageWindow = Utils.FindUIElementFrom<ItemStorageWindow>(target);
      if (storageWindow != null) {
        targetContainer = storageWindow;
        return (this.IsDropTarget(storageWindow));
      }
      targetContainer = null;
      return (false);
    }
  }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  class ItemBox: VisualElement, IHideableWindow
  {
    public ItemAndCount ItemData { get; private set; }
    VisualElement itemImage;
    Label itemLabel;
    public bool IsVisiable { get; private set; }

    public ItemBox(ItemAndCount itemData)
    { 
      this.ItemData = itemData;
      this.CreateUI();
      this.AddToClassList("inventory-window-item-box");
    }

    public void UpdateOffset(Vector2 offset)
    {
      this.style.translate = new Translate(offset.x, offset.y);
    }

    void CreateUI()
    {
      this.AddToClassList("inventory-window-item-box");
      this.itemImage = new VisualElement();
      this.itemImage.AddToClassList("inventory-window-item-image"); 
      this.itemLabel = new Label();
      this.itemLabel.AddToClassList("inventory-window-item-label");
      this.itemImage.style.backgroundImage = new StyleBackground(this.ItemData.Item.Image);
      if (this.ItemData.Count < 2) {
        itemLabel.text = this.ItemData.Item.Name;
      }
      else {
        itemLabel.text = String.Format($"{this.ItemData.Item.Name} ({this.ItemData.Count})");
      }
      this.Add(itemImage);
      this.Add(itemLabel);
    }

    public void Show()
    {
      this.IsVisiable = true;
      this.visible = true;
      this.BringToFront();
      
    }

    public void Hide()
    {
      this.IsVisiable = false;
      this.visible = false;
      this.SendToBack();
    }
  }

  public class InventoryWindow : VisualElement, IHideableWindow
  {
    public bool IsVisiable { get; private set; }
    VisualElement itemsContainer;
    public Action<ItemAndCount> OnStartDragItem;
    VisualElement currentDraggingTarget;
    bool IsDraggingItem => this.currentDraggingTarget != null;
    Dictionary<VisualElement, ItemAndCount> itemBoxTable;
    Vector2 dragStartPosition;

    public InventoryWindow()
    {
      this.name = "inventory-window-container";
      this.itemBoxTable = new ();
      this.AddToClassList("window-container");
      this.CreateUI();
    }

    public void Show()
    {
      Inventory.Instance.OnChanged += this.OnInventoryUpdated;
      this.OnInventoryUpdated(Inventory.Instance);
      this.IsVisiable = true;
      this.visible = true;
      this.BringToFront();
    }

    public void Hide()
    {
      Inventory.Instance.OnChanged -= this.OnInventoryUpdated;
      this.IsVisiable = false;
      this.visible = false;
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
      label.text = "Invetory";
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
        var box = this.CreateItembox(
          new ItemAndCount { Item = item, Count = count });
        this.itemsContainer.Add(box);
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
      ItemBox itemBox = new ItemBox(itemAndCount);
      itemBox.RegisterCallback<PointerDownEvent>(this.OnPointerDown);         
      itemBox.RegisterCallback<PointerUpEvent>(this.OnPointerUp);
      itemBox.RegisterCallback<PointerMoveEvent>(this.OnPointerMove);
      this.itemBoxTable[itemBox] = itemAndCount;
      return (itemBox);
    }

    ItemBox FindItemBoxFrom(VisualElement target)
    {
      if (target is ItemBox itemBox) {
        return (itemBox);
      }
      return (target.GetFirstAncestorOfType<ItemBox>());
    }

    void OnPointerDown(PointerDownEvent pointerDownEvent)
    {
      if (!this.IsDraggingItem) {
        var boxElement = this.FindItemBoxFrom(pointerDownEvent.target as VisualElement);
        this.dragStartPosition = pointerDownEvent.position;
        if (boxElement != null &&
          this.itemBoxTable.TryGetValue(boxElement,
            out ItemAndCount itemData)) {
          boxElement.AddToClassList("inventory-window-floating-itembox");
          this.currentDraggingTarget = boxElement;
          boxElement.CapturePointer(pointerDownEvent.pointerId);
          this.OnStartDragItem?.Invoke(itemData);
        }
        else {
          Debug.LogError($"Pointer target is not in ItemBox or itemBoxTable");
        }
      }
      else {
        Debug.Log($"Already dragging item");
      }
    }

    void OnPointerMove(PointerMoveEvent pointerMoveEvent)
    {
      if (this.IsDraggingItem) {
        var boxElement = this.FindItemBoxFrom(pointerMoveEvent.target as VisualElement);
        if (boxElement == this.currentDraggingTarget &&
          boxElement is ItemBox itemBox) {
          var offset = new Vector2(
            pointerMoveEvent.position.x,
            pointerMoveEvent.position.y) - this.dragStartPosition;
          itemBox.UpdateOffset(offset); 
        }
      } 
    }

    void OnPointerUp(PointerUpEvent pointerUpEvent)
    {
      if (pointerUpEvent.target == this.currentDraggingTarget) {
        this.currentDraggingTarget.RemoveFromClassList("inventory-window-floating-itembox");
        if (this.currentDraggingTarget is ItemBox itemBox) {
          itemBox.UpdateOffset(Vector2.zero);
        }
        this.currentDraggingTarget.ReleasePointer(pointerUpEvent.pointerId);
        this.currentDraggingTarget = null;
      }
    }
  }
}

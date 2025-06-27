using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class ItemStorageWindow : ItemConatinerWindow
  {
    public ItemStorageWindow(ItemBox floatingItemBox) : base(floatingItemBox, App.Instance.ItemStorage)
    {
    }

    protected override void ClearItems()
    {
      this.itemsContainer.Clear();
    }

    protected override void CreateUI()
    {
    }

    protected override void DropItem(ItemAndCount itemAndCount, ItemConatinerWindow targetContainer)
    {
      if (targetContainer is InventoryWindow inventoryItemContainer) {

        if (itemAndCount.Count > 1) {
          var (item, count) = this.ItemSource.GetItems(itemAndCount.Item, itemAndCount.Count);
          targetContainer.ItemSource.AddItems(item, count);
        }
        else {
          var item = this.ItemSource.GetItem(itemAndCount.Item);
          targetContainer.ItemSource.AddItem(item);
        }
      }
    }

    protected override void DropItemOutSide(ItemAndCount itemAndCount)
    {
      throw new NotImplementedException();
    }

    protected override void FillItems(ItemStorageBase inventory)
    {
      foreach (var itemAndCount in inventory.Items) {
        var (item, count) = itemAndCount;
        if (count > 0) {
          var box = this.CreateItembox(
            new ItemAndCount { Item = item, Count = count });
          this.itemsContainer.Add(box);
        }
      }
    }
    //TODO: 각 아이템 UI를 objectpool에 보관
    ItemBox CreateItembox(ItemAndCount itemAndCount)
    {
      ItemBox itemBox = new ItemBox(this);
      itemBox.SetData(itemAndCount);
      itemBox.RegisterCallback<PointerDownEvent>(this.OnPointerDown);         
      itemBox.RegisterCallback<PointerUpEvent>(this.OnPointerUp);
      itemBox.RegisterCallback<PointerMoveEvent>(this.OnPointerMove);
      return (itemBox);
    }

    protected override bool IsAbleToDropItem(ItemData item, ItemConatinerWindow targetContainer)
    {
      if (targetContainer is InventoryWindow inventoryItemContainer) {
        return (true);
      }
      return (false);
    }

    protected override bool IsAbleToDropOutSide(ItemData item)
    {
      return (false);
    }

    protected override void OnUsePointerButtonDown(ItemBox boxElement, ItemAndCount itemAndCount)
    {
      throw new NotImplementedException();
    }
  }

  public class ItemStorageContainerWindow : VisualElement, IHideableUI
  {
    public bool IsVisiable { get; private set; }
    public ItemStorageWindow ItemContainer { get; private set; }
    ItemBox floatingBox;

    public ItemStorageContainerWindow(ItemBox floatingBox)
    {
      this.floatingBox = floatingBox;
      this.name = "item-storage-window-container";
      this.AddToClassList("window-container");
      this.CreateUI();
    }

    public void Hide()
    {
      this.IsVisiable = false;
      Utils.HideVisualElement(this);
      this.ItemContainer.Hide();
    }

    public void Show()
    {
      this.IsVisiable = true;
      Utils.ShowVisualElement(this);
      this.ItemContainer.Show(); 
    }

    void CreateUI()
    {
      var label = new Label();
      label.text = "Item Storage";
      label.AddToClassList("window-label");
      this.Add(label);
      var closeButton = new Button();
      closeButton.text = "close";
      closeButton.AddToClassList("window-close-button");
      closeButton.RegisterCallback<ClickEvent>(this.OnClickClose);
      this.Add(closeButton); 
      this.ItemContainer = new ItemStorageWindow(
        this.floatingBox
        );
      this.ItemContainer.name = "item-storage-item-container";
      this.Add(this.ItemContainer);
    }

    bool IsStorable(ItemData item)
    {
      return (true);
    }

    void OnClickClose(ClickEvent click)
    {
      this.Hide();
    }
  }
}

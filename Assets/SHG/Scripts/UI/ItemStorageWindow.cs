using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class ItemStorageInnerWindow : ItemConatinerWindow
  {
    public ItemStorageInnerWindow(ItemBox floatingItemBox) : base(floatingItemBox)
    {
    }

    protected override void ClearItems()
    {
      this.itemsContainer.Clear();
    }

    protected override void CreateUI()
    {
      throw new NotImplementedException();
    }

    protected override void DropItem(ItemAndCount itemAndCount)
    {
      throw new NotImplementedException();
    }

    protected override void DropItemOutSide(ItemAndCount itemAndCount)
    {
      throw new NotImplementedException();
    }

    protected override void FillItems(ItemStorageBase inventory)
    {
      throw new NotImplementedException();
    }

    protected override bool IsAbleToDropItem(ItemData item)
    {
      throw new NotImplementedException();
    }

    protected override bool IsAbleToDropOutSide(ItemData item)
    {
      throw new NotImplementedException();
    }

    protected override void OnUsePointerButtonDown(ItemBox boxElement, ItemAndCount itemAndCount)
    {
      throw new NotImplementedException();
    }
    }

    public class ItemStorageWindow : VisualElement, IHideableUI
  {
    public bool IsVisiable { get; private set; }
    ItemStorageInnerWindow ItemContainer;
    ItemBox floatingBox;

    public ItemStorageWindow(ItemBox floatingBox)
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
    }

    public void Show()
    {
      this.IsVisiable = true;
      Utils.ShowVisualElement(this);
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
      this.ItemContainer = new ItemStorageInnerWindow(
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

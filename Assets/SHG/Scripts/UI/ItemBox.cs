using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class ItemBox: VisualElement, IHideableWindow
  {
    public ItemAndCount ItemData { get; private set; }
    VisualElement itemImage;
    Label itemLabel;
    public bool IsVisiable { get; private set; }
    public VisualElement ParentWindow { get; private set; }

    public ItemBox(VisualElement window)
    { 
      this.ParentWindow = window;
      this.CreateUI();
      this.AddToClassList("inventory-window-item-box");
    }

    public static ItemBox FindItemBoxFrom(VisualElement target)
    {
      if (target == null) {
        return (null);
      }
      if (target is ItemBox itemBox) {
        return (itemBox);
      }
      return (target.GetFirstAncestorOfType<ItemBox>());
    }

    public void SetData(ItemAndCount itemData)
    {
      this.ItemData = itemData;
      this.itemImage.style.backgroundImage = new StyleBackground(this.ItemData.Item.Image);
      if (this.ItemData.Count < 2) {
        itemLabel.text = this.ItemData.Item.Name;
      }
      else {
        itemLabel.text = String.Format($"{this.ItemData.Item.Name} ({this.ItemData.Count})");
      }
    }

    public void RemoveData()
    {
      this.ItemData = ItemAndCount.None;
      this.itemImage.style.backgroundImage = null;
      this.itemLabel.text = "";
    }

    public void UpdateOffset(Vector2 offset)
    {
      this.style.translate = new Translate(offset.x, offset.y);
    }

    void CreateUI()
    {
      this.AddToClassList("inventory-item-box");
      this.itemImage = new VisualElement();
      this.itemImage.AddToClassList("inventory-window-item-image"); 
      this.itemLabel = new Label();
      this.itemLabel.AddToClassList("inventory-window-item-label");
      this.Add(itemImage);
      this.Add(itemLabel);
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

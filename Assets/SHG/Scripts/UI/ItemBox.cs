using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class ItemBox: VisualElement, IHideableUI
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
      this.AddToClassList("item-storage-item-box");
    }

    public void SetData(ItemAndCount itemData)
    {
      this.ItemData = itemData;
      this.itemImage.style.backgroundImage = new StyleBackground(this.ItemData.Item.Image);
      if (this.ItemData.Count < 2) {
        this.itemLabel.text = this.ItemData.Item.Name;
      }
      else {
        this.itemLabel.text = String.Format($"{this.ItemData.Item.Name} ({this.ItemData.Count})");
      }
    }

    public void SetLabelText(string text)
    {
      this.itemLabel.text = text; 
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
      this.AddToClassList("item-storage-item-box");
      this.itemImage = new VisualElement();
      this.itemImage.AddToClassList("item-box-item-image"); 
      this.itemLabel = new Label();
      this.itemLabel.AddToClassList("item-box-item-label");
      this.Add(itemImage);
      this.Add(itemLabel);
    }

    public void Show()
    {
      this.IsVisiable = true;
      Utils.ShowVisualElement(this);
    }

    public void Hide()
    {
      this.IsVisiable = false;
      Utils.HideVisualElement(this);
    }
  }
}

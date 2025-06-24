using System;
using UnityEngine.UIElements;

namespace SHG
{
  public class InventoryWindow : VisualElement, IHideableWindow
  {
    public bool IsVisiable { get; private set; }
    VisualElement itemsContainer;

    public InventoryWindow()
    {
      this.name = "inventory-window-container";
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
        var box = this.CreateItembox(item, count);
        this.itemsContainer.Add(box);
      } 
    }

    // TODO: return to Obejctpool
    void ClearInventoryItems()
    {
      this.itemsContainer.Clear();
    }

    //TODO: 각 아이템 UI를 objectpool에 보관
    VisualElement CreateItembox(ItemData itemData, int count)
    {
      if (itemData.Image == null) {
        throw (new ArgumentException($"{itemData.Name} has no image for ui"));
      }
      VisualElement box = new VisualElement();
      box.AddToClassList("inventory-window-item-box");
      VisualElement itemImage = new VisualElement();
      itemImage.style.backgroundImage = new StyleBackground(itemData.Image);
      itemImage.AddToClassList("inventory-window-item-image"); 
      box.Add(itemImage);
      var itemLabel = new Label();
      if (count < 2) {
        itemLabel.text = itemData.Name;
      }
      else {
        itemLabel.text = String.Format($"{itemData.Name} ({count})");
      }
      itemLabel.AddToClassList("inventory-window-item-label");
      box.Add(itemLabel);
      return (box);
    }
  }
}

using System;
using UnityEngine.UIElements;

namespace SHG
{
  class SlotItem: VisualElement
  {
    public ItemData itemData { get; private set; }
    public bool IsEmpty => (this.itemData == null);
    VisualElement itemImage;
    Label itemLabel;

    public SlotItem() 
    { 
      this.CreateUI();
    }

    public void SetData(ItemData itemData)
    {
      this.itemData = itemData;
      this.itemImage.style.backgroundImage = new StyleBackground(itemData.Image);
      this.itemLabel.text = String.Format($"{itemData.Name}");
    }

    public void RemoveData()
    {
      this.itemData = null;
      this.itemImage.style.backgroundImage = null;
      this.itemLabel.text = null;
    }

    void CreateUI()
    {
      this.AddToClassList("quick-slot-window-item-container");
      this.itemImage = new VisualElement();
      this.itemImage.AddToClassList("inventory-window-item-image"); 
      this.Add(this.itemImage);
      this.itemLabel = new Label();
      this.itemLabel.AddToClassList("inventory-window-item-label");
      this.Add(this.itemLabel);
    }
      
  }
  public class QuickSlotWindow : VisualElement, IHideableWindow
  {
    public bool IsVisiable { get; private set; }
    SlotItem[] slots;

    public QuickSlotWindow()
    {
      this.name = "quick-slot-window";     
      this.AddToClassList("window-container");
      this.CreateUI();
      Inventory.Instance.OnChanged += this.OnChangeInventory;
      this.OnChangeInventory(Inventory.Instance);
    }

    void OnChangeInventory(Inventory inventory)
    {
      var itemCount = inventory.QuickSlotItems.Count;
      for (int i = 0; i < this.slots.Length; i++) {
        if (i < itemCount) {
          var itemData = inventory.QuickSlotItems[i];
          if (itemData != null) {
            this.slots[i].SetData(itemData);
          }
          else {
            this.slots[i].RemoveData();
          }
        }
        else {
          this.slots[i].RemoveData();
        }
      } 
    }

    void CreateUI()
    {
      this.slots = new SlotItem[Inventory.QUICKSLOT_COUNT];
      for (int i = 0; i < this.slots.Length; i++) {
        this.slots[i] = new SlotItem();
        this.Add(this.slots[i]);
      }
    }

    VisualElement CreateItemBox(ItemData itemData)
    {
      var itemContainer = new VisualElement();
      itemContainer.AddToClassList("quick-slot-window-item-container");
      var itemImage = new VisualElement();
      itemImage.AddToClassList("inventory-window-item-image"); 
      var itemLabel = new Label();
      itemLabel = new Label();
      itemLabel.AddToClassList("inventory-window-item-label");
      itemImage.style.backgroundImage = new StyleBackground(itemData.Image);
      itemLabel.text = String.Format($"{itemData.Name}");
      return (itemContainer);
    }

    public void Show()
    {
      this.IsVisiable = true;
      this.visible = true;
    }

    public void Hide()
    {
      this.IsVisiable = false;
      this.visible = false;
    }
  }
}

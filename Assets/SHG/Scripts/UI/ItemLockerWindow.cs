using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class ItemLockerWindow : ItemStorageWindow
  {
    protected override Vector2 DescriptionOffset => this.DESCRIPTION_OFFSET;
    readonly Vector2 DESCRIPTION_OFFSET = new Vector2(0, 150f);
    public ItemLockerWindow(ItemBox floatingItemBox) : base(floatingItemBox, App.Instance.ItemStorage)
    {
      this.name = "item-locker-window";
    }

    protected override void ClearItems()
    {
      this.itemsContainer.Clear();
    }

    protected override void CreateUI()
    {
      this.itemDescriptionContainer = new VisualElement();
      this.itemDescriptionContainer.AddToClassList("item-storage-item-description-container");
      this.itemDescription = new Label();
      this.itemDescription.AddToClassList("item-storage-item-description");
      this.itemDescriptionContainer.Add(this.itemDescription);
      this.Add(this.itemDescriptionContainer);
      Utils.HideVisualElement(this.itemDescriptionContainer);
    }

    protected override void DropItem(ItemAndCount itemAndCount, ItemStorageWindow targetContainer)
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
      foreach (var pair in inventory.Items) {
        var (item, count) = pair;
        if (item.IsStoryItem) {
          for (int i = 0; i < count; i++) {
          var box = this.CreateItembox(
            new ItemAndCount { Item = item, Count = 1 });
          this.itemsContainer.Add(box);
          } 
        }
        else {
          var stackCount = count / inventory.MAX_STACK_COUNT; 
          var restCount = count % inventory.MAX_STACK_COUNT;
          for (int i = 0; i < stackCount; i++) {
            var box = this.CreateItembox(
              new ItemAndCount { 
              Item = item, Count = inventory.MAX_STACK_COUNT });
            this.itemsContainer.Add(box);
             
          }
          if (restCount != 0) {
            var box = this.CreateItembox(
              new ItemAndCount { 
              Item = item, Count = restCount });
            this.itemsContainer.Add(box);
          }
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
      itemBox.RegisterCallback<PointerOverEvent>(this.OnPointerOver);
      itemBox.RegisterCallback<PointerLeaveEvent>(this.OnPointerLeave);
      return (itemBox);
    }

    protected override bool IsAbleToDropItem(ItemData item, ItemStorageWindow targetContainer)
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

  public class ItemLockerContainerWindow : VisualElement, IHideableUI
  {
    public bool IsVisiable { get; private set; }
    public ItemLockerWindow ItemContainer { get; private set; }
    ItemBox floatingBox;

    public ItemLockerContainerWindow(ItemBox floatingBox)
    {
      this.floatingBox = floatingBox;
      this.name = "item-locker-window-container";
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
      this.ItemContainer = new ItemLockerWindow(
        this.floatingBox
        );
      this.ItemContainer.name = "item-locker-item-container";
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

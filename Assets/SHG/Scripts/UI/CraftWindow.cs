using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class CraftWindow : VisualElement, IHideableUI
  {
    public static CraftProvider CurrentProvider { get; set; }
    ProductListWindow productListWindow;
    RecipesWindow recipeWindow;
    public bool IsVisiable { get; private set; }

    public CraftWindow(ItemBox floatingBox)
    {
      this.name = "craft-window-container";
      this.AddToClassList("window-container");
      this.CreateUI();
      this.productListWindow.OnClickItem += this.OnProductSelected;
    }

    void OnProductSelected(ItemData product)
    {
      this.recipeWindow.ShowProductRecipes(product); 
    }

    void CreateUI()
    {
      var label = new Label();
      label.AddToClassList("window-label");
      label.text = "Craft";
      this.Add(label);
      var closeButton = new Button();
      closeButton.AddToClassList("window-close-button");
      closeButton.RegisterCallback<ClickEvent>(this.OnClickCloseButton);
      this.Add(closeButton);
      var scrollView = new ScrollView(ScrollViewMode.Vertical);
      scrollView.AddToClassList("item-container-scroll-view");
      this.productListWindow = new ProductListWindow();   
      scrollView.Add(this.productListWindow);
      this.Add(scrollView);
      this.recipeWindow = new RecipesWindow();
      this.Add(this.recipeWindow);
    }

    void OnClickCloseButton(ClickEvent click)
    {
      this.Hide();
    }

    void OnInventoryUpdated(ItemStorageBase inventory)
    {
      this.productListWindow.UpdateProductsEnable();
      if (this.productListWindow.SelectedItem != null &&
        this.productListWindow.SelectedItem.ItemData != ItemAndCount.None) {
        this.recipeWindow.ClearPresentingData();
        this.recipeWindow.ShowProductRecipes(this.productListWindow.SelectedItem.ItemData.Item);
      }
    }

    public void Hide()
    {
      this.IsVisiable = false;
      Utils.HideVisualElement(this);
      this.productListWindow.SelectedItem = null;
      this.recipeWindow.ClearPresentingData();
      App.Instance.Inventory.OnChanged -= this.OnInventoryUpdated;
    }

    public void Show()
    {
      this.IsVisiable = true;
      Utils.ShowVisualElement(this);
      App.Instance.Inventory.OnChanged += this.OnInventoryUpdated;
    }
  }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class ProductListWindow: VisualElement
  {
    public Action<ItemData> OnClickItem;
    List<ItemBox> productBoxes;
    public ItemBox SelectedItem;

    public ProductListWindow()
    {
      this.name = "product-list-window-container";
      this.productBoxes = new ();
      this.CreateUI();
      this.FillProducts();
    }

    public void UpdateProductsEnable()
    {
      foreach (var productBox in this.productBoxes) {
        if (productBox.ItemData != ItemAndCount.None) {
          var recipes = App.Instance.Inventory.GetCraftableRecipes(productBox.ItemData.Item);
          if (recipes.Count == 0) {
            productBox.AddToClassList("product-list-item-box-inactive");
          }
          else {
            productBox.RemoveFromClassList("product-list-item-box-inactive");
          }
        } 
      }
    }

    void CreateUI()
    {
      for (int i = 0; i < RecipeRegistry.NUMBER_OF_PRODUCTS; i++) {
        var itemBox = this.CreateItemBox();
        this.productBoxes.Add(itemBox);
        this.Add(itemBox); 
      } 
    }

    ItemBox CreateItemBox()
    {
      ItemBox itemBox = new ItemBox(this);
      itemBox.AddToClassList("product-list-item-box");
      itemBox.RegisterCallback<ClickEvent>(this.OnClickItemBox);
      return (itemBox);
    }

    void FillProducts()
    {
      var products = RecipeRegistry.Instance.GetAllProducts();
      int index = 0;
      foreach (var product in products) {
        if (index >= this.productBoxes.Count - 1) {
          #if UNITY_EDITOR
          Debug.LogError($"number of products is more than {RecipeRegistry.NUMBER_OF_PRODUCTS}");
          #endif
          var newBox = this.CreateItemBox();
          this.productBoxes.Add(newBox);
          this.Add(newBox);
        }
        this.productBoxes[index].SetData(new ItemAndCount {
          Item = product, Count = 1
          });
        var recipes = App.Instance.Inventory.GetCraftableRecipes(product);
        if (recipes.Count == 0) {
          this.productBoxes[index].AddToClassList("product-list-item-box-inactive");
        }
        else {
          this.productBoxes[index].RemoveFromClassList("product-list-item-box-inactive");
        }
        index += 1;
      }
    }

    void OnClickItemBox(ClickEvent click)
    {
      var itemBox = Utils.FindUIElementFrom<ItemBox>(click.target as VisualElement);
      if (itemBox != null && itemBox.ItemData != ItemAndCount.None) {
        if (this.SelectedItem != null) {
          this.SelectedItem.RemoveFromClassList("product-list-item-box-selected");
        }
        this.SelectedItem = itemBox;
        this.SelectedItem.AddToClassList("product-list-item-box-selected");
        this.OnClickItem?.Invoke(itemBox.ItemData.Item); 
      }
    }
  }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class ProductListWindow: VisualElement
  {
    const int NUMBER_OF_PRODUCTS = 10;
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

    void CreateUI()
    {
      for (int i = 0; i < NUMBER_OF_PRODUCTS; i++) {
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
        if (index >= this.productBoxes.Count) {
          Debug.LogError("number of products is out of range");
          break;
        }
        this.productBoxes[index].SetData(new ItemAndCount {
          Item = product, Count = 1
          });
        var recipes = Inventory.Instance.GetCraftableRecipes(product);
        if (recipes.Count == 0) {
          this.productBoxes[index].AddToClassList("product-list-item-box-inactive");
        }
        else {
          this.productBoxes[index].RemoveFromClassList("product-list-item-box-inactive");
        }
      }
    }

    void OnClickItemBox(ClickEvent click)
    {
      var itemBox = Utils.FindUIElementFrom<ItemBox>(click.target as VisualElement);
      if (itemBox != null && itemBox.ItemData != ItemAndCount.None) {
        this.SelectedItem = itemBox;
        this.OnClickItem?.Invoke(itemBox.ItemData.Item); 
      }
    }
  }
}

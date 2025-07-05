using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class RecipeView: VisualElement, IHideableUI
  {
    public Action<ItemRecipe> OnClickCraft;
    ItemRecipe recipe;
    ItemBox productBox;
    VisualElement materialList;
    Button craftButton;

    public bool IsVisiable { get; private set; }

    public RecipeView()
    {
      this.AddToClassList("recipe-view-container");
      this.CreateUI();
    }

    public void SetRecipe(ItemRecipe recipe)
    {
      this.recipe = recipe;
      this.ClearPresentingData();
      this.productBox.SetData(new ItemAndCount { Item = recipe.RecipeData.Product, Count = 1 });
      this.productBox.SetLabel(recipe.RecipeData.Product.Name);
      this.PresentRecipe(recipe);
    }

    void PresentRecipe(ItemRecipe recipe)
    {
      bool isCraftable = App.Instance.PlayerStatManager.Fatigue.FatigueCur > 5f;
      foreach (var itemAndCount in recipe.RequiredItems) {
        ItemBox itemBox = new ItemBox(this);
        var currentItemCount = App.Instance.Inventory.GetItemCount(itemAndCount.Item);
        itemBox.SetData(itemAndCount);
        itemBox.SetLabel($"{itemAndCount.Item.Name} {currentItemCount}/{itemAndCount.Count}");
        var label = itemBox.Q<Label>();
        label.ClearClassList();
        label.AddToClassList("recipe-view-item-box-label");
        if (currentItemCount < itemAndCount.Count) {
          isCraftable = false;
          itemBox.AddToClassList("recipe-view-material-box-inactive");
        }
        else {
          itemBox.RemoveFromClassList("recipe-view-material-box-inactive");
        }
        itemBox.SetLabel($"{itemAndCount.Item.Name} {currentItemCount}/{itemAndCount.Count}");
        this.materialList.Add(itemBox);
      }
      if (isCraftable) {
        this.craftButton.SetEnabled(true);
        this.craftButton.RemoveFromClassList("recipe-view-craft-button-disabled");
        this.productBox.RemoveFromClassList("recipe-view-product-box-inactive");
      }
      else {
        this.craftButton.SetEnabled(false);
        this.craftButton.AddToClassList("recipe-view-craft-button-disabled");
        this.productBox.AddToClassList("recipe-view-product-box-inactive");
      }
    }

    public void ClearPresentingData()
    {
      this.productBox.RemoveData(); 
      this.materialList.Clear();
    }

    void CreateUI()
    {
      var productBoxContainer = new VisualElement();
      productBoxContainer.AddToClassList("recipe-view-product-box-container");
      this.productBox = new ItemBox(this);   
      var label = this.productBox.Q<Label>();
      label.ClearClassList();
      label.AddToClassList("recipe-view-item-box-label");
      this.productBox.AddToClassList("recipe-view-product-box");
      productBoxContainer.Add(this.productBox);
      this.Add(productBoxContainer);
      this.materialList = new VisualElement();
      this.materialList.AddToClassList("recipe-view-material-list");
      this.Add(this.materialList);
      var buttonContainer = new VisualElement();
      buttonContainer.AddToClassList("recipe-view-craft-button-container");
      this.craftButton = new Button();
      this.craftButton.text = "제작";
      this.craftButton.RegisterCallback<ClickEvent>(this.OnClickCraftButton);
      this.craftButton.AddToClassList("recipe-view-craft-button");
      buttonContainer.Add(this.craftButton);
      this.Add(buttonContainer);
    }

    void OnClickCraftButton(ClickEvent click)
    {
      if (this.recipe != null) {
        this.OnClickCraft?.Invoke(this.recipe);
      }
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

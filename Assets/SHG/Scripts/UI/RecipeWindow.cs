using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public class RecipesWindow: VisualElement
  {
    List<RecipeView> recipeViews;

    public RecipesWindow()
    {
      this.recipeViews = new ();
      this.name = "recipes-window";
      this.AddToClassList("window-container");
    }

    public void ShowProductRecipes(ItemData product)
    {
      this.ClearPresentingData();
      this.PresentRecipeForProduct(product);
    }

    void OnClickCraft(ItemRecipe recipe)
    {
      try {
        var currentFatigue = App.Instance.PlayerStatManager.Fatigue.FatigueCur;
        if (currentFatigue < 5f) {
          return ;
        }
        App.Instance.PlayerStatManager.Fatigue.SetFatigue(
          currentFatigue - 5f);
        var craftedItem = App.Instance.Inventory.CraftItem(recipe);
        App.Instance.Inventory.AddItem(craftedItem);
      }
      catch (Exception e) {
        #if UNITY_EDITOR
        Debug.LogError($"Fail to craft {recipe.RecipeData.Product.Name} {e.Message}");
        #endif
      }
    }

    public void ClearPresentingData()
    {
      foreach (var recipeView in this.recipeViews) {
        recipeView.ClearPresentingData(); 
        recipeView.Hide();
      }
    }

    void PresentRecipeForProduct(ItemData product)
    {
      var recipes = RecipeRegistry.Instance.GetRecipes(product);
      if (recipes.Count == 0) {
        throw (new ApplicationException($"No recipe found for {product.Name}"));
      }
      while (this.recipeViews.Count < recipes.Count) {
        var recipeView = new RecipeView();
        recipeView.OnClickCraft += this.OnClickCraft;
        this.recipeViews.Add(recipeView);
        this.Add(recipeView);
      }
      var index = 0;
      this.PresentRecipe(recipes[0], this.recipeViews[index]);
    }

    void PresentRecipe(ItemRecipe recipe, RecipeView recipeView)
    {
      recipeView.SetRecipe(recipe);
      recipeView.Show();
    }
  }
}

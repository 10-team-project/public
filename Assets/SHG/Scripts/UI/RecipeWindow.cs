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
        var craftedItem = App.Instance.Inventory.CraftItem(recipe);
        App.Instance.Inventory.AddItem(craftedItem);
      }
      catch (Exception e) {
        Debug.LogError($"Fail to craf {recipe.RecipeData.Product.Name} {e.Message}");
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
      var recipes = RecipeRegistry.Instance.GetRecipes(product, CraftWindow.CurrentProvider);
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

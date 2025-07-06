using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class CraftTest : MonoBehaviour
  {
  
    [SerializeField]
    ItemData itemToCraft;
    [SerializeField]
    ItemRecipeData selectedRecipeData;

    [Button ("Open craft")]
    void OpenCraftWindow()
    {
      App.Instance.UIController.OpenCraftWindow();
    }

    [Button("Get recipes test")]
    void GetRecipe()
    {
      if (itemToCraft == null) {
        Debug.Log("No Item is selected");
        return ;
      }
      var recipes = RecipeRegistry.Instance.GetRecipes(this.itemToCraft);
      
      Debug.Log($"found {recipes.Count} recipes for {this.itemToCraft.Name}");
      this.PrintRecipes(recipes);
    }

    [Button ("Get current craftable recipes")]
    void GetCraftableRecipes()
    {
      if (itemToCraft == null) {
        Debug.Log("No Item is selected");
        return ;
      }
      var recipes = App.Instance.Inventory.GetCraftableRecipes(this.itemToCraft);
      Debug.Log($"craftable recipe: {recipes.Count} for {this.itemToCraft.Name} from current inventory");
      this.PrintRecipes(recipes);
    }

    [Button("Craft test")]
    void CraftItem()
    {
      if (this.selectedRecipeData == null) {
        Debug.Log("No recipe to craft");
        return ;
      }
      var recipe = new ItemRecipe(this.selectedRecipeData);
      try {
        var item = App.Instance.Inventory.CraftItem(recipe);      
        Item.CreateItemObjectFrom(item);
      }
      catch (Exception e) {
        Debug.LogError($"Fail to Craft: {e.Message}");
      }
    }

    void PrintRecipes(List<ItemRecipe> recipes)
    {
      var index = 0;
      recipes.ForEach(
        recipe => {
          var builder = new StringBuilder($"{++index} :");
          foreach (var itemAndCount in recipe.RequiredItems) {
            builder.Append($"[{itemAndCount.Item.Name}: {itemAndCount.Count}]");
          }
          Debug.Log(builder.ToString());
        });
    }
  }
}

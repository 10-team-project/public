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
    [SerializeField, ReadOnly]
    List<string> recipesForCraft;

    [Button("Get recipes test")]
    void GetRecipe()
    {
      if (itemToCraft == null) {
        Debug.Log("No Item is selected");
        return ;
      }
      var recipes = RecipeRegistry.Instance.GetRecipes(this.itemToCraft);
      
      Debug.Log($"found {recipes.Count} recipes for {this.itemToCraft.Name}");
      var index = 0;
      this.recipesForCraft = recipes.ConvertAll<string>(
        recipe => {
          var builder = new StringBuilder($"{++index} :");
          foreach (var itemAndCount in recipe.RequiredItems) {
            builder.Append($"[{itemAndCount.Item.Name}: {itemAndCount.Count}]");
          }
          return (builder.ToString());
        });
    }
  }
}

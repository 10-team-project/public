using UnityEngine;
using EditorAttributes;
using SHG;

public partial class ItemData 
{
  public bool IsCraftable => this.Recipes != null && this.Recipes.Length > 1;
  [HideInInspector]
  public ItemRecipeData[] Recipes => this.recipes;

  [SerializeField, Validate("Recipe are invalid, Product is not matched", nameof(IsRecipesProductValid), MessageMode.Error)]
  protected ItemRecipeData[] recipes;

  protected bool IsRecipesProductValid()
  {
    foreach (var recipe in this.Recipes) {
      if (recipe == null || recipe.Product != this) {
        return (true);
      } 
    }
    return (false);
  }
}

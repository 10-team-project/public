using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [CreateAssetMenu (menuName = "ScriptableObjects/Items/CraftableItem")]
  public class CraftableItemData : ItemData
  {
    public ItemRecipeData[] Recipes => this.recipes;

    [SerializeField, Validate("Empty Recipe", nameof(IsRecipeEmpty), MessageMode.Error)]
    protected ItemRecipeData[] recipes;

    [SerializeField, ReadOnly, Validate("Recipe are invalid, Product is not matched", nameof(IsRecipesProductValid), MessageMode.Error)]
    protected bool IsRecipesProductSame;

    protected bool IsRecipeEmpty() => (this.recipes == null || this.recipes.Length == 0);

    protected bool IsRecipesProductValid()
    {
      foreach (var recipe in this.Recipes) {
        if (recipe.Product != this) {
          return (true);
        } 
      }
      return (false);
    }
  }
}

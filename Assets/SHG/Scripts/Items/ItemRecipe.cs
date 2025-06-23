using System.Collections.Generic;

namespace SHG
{
  public struct ItemAndCount
  {
     public ItemData Item;
     public int Count;
  }

  public class ItemRecipe: UnityEngine.Object
  {
    public ItemRecipeData RecipeData { get; private set; }
    public List<ItemAndCount> RequiredItems { get; private set; }

    public ItemRecipe(ItemRecipeData recipeData)
    {
      this.RecipeData = recipeData;
      this.RequiredItems = new ();
      foreach (var material in recipeData.Materials) {
        int index = RequiredItems.FindIndex(
            itemAndCount => itemAndCount.Item == material
            );
        if (index == -1)  {
          this.RequiredItems.Add(new ItemAndCount { Item = material, Count = 1 });
        }
        else {
          var currentCount = this.RequiredItems[index].Count;
          this.RequiredItems[index] = new ItemAndCount { Item = material, Count = currentCount + 1 };
        }
      }
    }
  }
}

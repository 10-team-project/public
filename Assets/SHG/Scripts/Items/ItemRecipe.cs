using System;
using System.Collections.Generic;

namespace SHG
{
  public struct ItemAndCount: IEquatable<ItemAndCount>
  {
    public ItemData Item;
    public int Count;
    public static ItemAndCount None = new ItemAndCount { Item = null, Count = 0 };

    public bool Equals(ItemAndCount other)
    {
      if (other is ItemAndCount otherItemAndCount) {
        return (otherItemAndCount.Item == this.Item && 
          otherItemAndCount.Count == this.Count);
      }
      return (false);
    }

    public override bool Equals(object obj)
    {
      if (obj is ItemAndCount itemAndCount) {
        return (itemAndCount == this);
      }
      return (false);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override string ToString()
    {
      return ($"{this.Item.Name}_{this.Count}");
    }

    public static bool operator== (ItemAndCount lhs, ItemAndCount rhs)
    {
      if (lhs != null && rhs != null)
      {
        return (lhs.Equals(rhs));
      }
      return (lhs == null && rhs == null);
    }
    public static bool operator!= (ItemAndCount lhs, ItemAndCount rhs)
    {
      return (!(lhs == rhs));
    }

  }

  [Serializable]
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

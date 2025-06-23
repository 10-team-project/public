using System;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using SHG;

public class Inventory : SingletonBehaviour<Inventory>, IObservableObject<Inventory>
{

  public Dictionary<ItemData, int> Items { get; private set; }
#if UNITY_EDITOR
  [SerializeField]
  public List<string> ItemNamesForDebugging;
#endif

  public Action<Inventory> WillChange { get; set; }
  public Action<Inventory> OnChanged { get; set; }

  public List<ItemRecipe> GetCraftableRecipes(ItemData product)
  {
    if (product.Recipes.Length == 0) {
      return (RecipeRegistry.EMPTY_RECIPES);
    }
    var recipes = RecipeRegistry.Instance.GetRecipes(product);
    if (recipes.Count == 0) {
      return (RecipeRegistry.EMPTY_RECIPES);
    }
    List<ItemRecipe> craftableRecipes = new ();
    foreach (var recipe in recipes) {
      var required = recipe.RequiredItems;
      bool isCraftable = true;
      for (int i = 0; i < required.Count; ++i) {
        var itemAndCount = required[i];
        var currentCount = this.GetItemCount(itemAndCount.Item);
        if (currentCount < itemAndCount.Count) {
          isCraftable = false;
          break;
        }
      }
      if (isCraftable) {
        craftableRecipes.Add(recipe);
      }
    }
    return (craftableRecipes);
  }

  public Item CraftItem(ItemRecipe recipe)
  {
    foreach (var required in recipe.RequiredItems) {
      if (!this.Items.TryGetValue(required.Item, out int count) ||
        count < required.Count) {
        throw (new ApplicationException($"not enough material for {recipe.RecipeData.Product}"));
      }  
      else {
        this.Items[required.Item] = count - required.Count;
      }
    } 
    return (Item.CreateItemFrom(recipe.RecipeData.Product));
  }

  public void AddItem(Item item)
  {
    this.WillChange?.Invoke(this);
#if UNITY_EDITOR
    this.AddItemName(item.Data);
#endif
    if (this.Items.TryGetValue(item.Data, out int itemCount)) {
      this.Items[item.Data] = itemCount + 1;
    }
    else {
      this.Items.Add(item.Data, 1);
    }
    this.OnChanged?.Invoke(this);
  }

#if UNITY_EDITOR
  void AddItemName(ItemData data) 
  {
    this.ItemNamesForDebugging.Add(data.Name);
    this.ItemNamesForDebugging.Sort();
  }
#endif

  public int GetItemCount(ItemData itemData)
  {
    if (this.Items.TryGetValue(itemData, out int itemCount)) {
      return (itemCount);
    }
    else {
      return (0);
    }
  }

  public Item GetItem(ItemData itemData)
  {
    int itemCount = this.GetItemCount(itemData);
    if (itemCount < 1) {
      throw (new ApplicationException($"GetItem: No {itemData.Name} in Inventory")); 
    }
    this.WillChange?.Invoke(this);
    Item item;
    if (itemData is RecoveryItemData recoveryItemData) {
      item = new RecoveryItem(recoveryItemData); 
    }
    else {
      throw (new NotImplementedException());
    }
#if UNITY_EDITOR
    var index = this.ItemNamesForDebugging.FindIndex(
      name => name == itemData.Name);
    if (index != -1) {
      this.ItemNamesForDebugging.RemoveAt(index);
    }
#endif
    if (itemCount > 0) {
      this.Items.Remove(itemData);
    }
    else {
      this.Items[itemData] = itemCount - 1;
    }
    this.OnChanged?.Invoke(this);
    return (item);
  }

  protected override void Awake()
  {
    base.Awake();
    this.Items = new ();
#if UNITY_EDITOR
    this.ItemNamesForDebugging = new();
#endif
  }
}

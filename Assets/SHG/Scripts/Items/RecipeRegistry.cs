using System.Collections.Generic;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class RecipeRegistry : SingletonBehaviour<RecipeRegistry>
  {
    const string ITEM_DIR = "Assets/SHG/Test/Items";
    const string RECIPE_DIR = "Assets/SHG/Test/Recipes";
    public static readonly List<ItemRecipe> EMPTY_RECIPES = new (0);
    //TODO: 아이템 레시피 최적화 자료구조
    Dictionary<ItemData, List<ItemRecipe>> recipeTable;
    #if UNITY_EDITOR
    List<ItemRecipe> recipes = new ();
    #endif
    
    public IEnumerable<ItemData> GetAllProducts()
    {
      return (this.recipeTable.Keys);
    }

    public List<ItemRecipe> GetRecipes(ItemData craftableItem)
    {
      if (this.recipeTable.TryGetValue(craftableItem, out List<ItemRecipe> recipes)) {
        return (recipes);
      }
      return (EMPTY_RECIPES);
    }

    protected override void Awake()
    {
      base.Awake();
      this.Init();
    }

    void Init()
    {
      this.recipeTable = new ();
      this.LoadAllItems();
      this.LoadAllRecipes();
    }

    void Start()
    {
      if (Inventory.Instance == null) {
        Debug.LogError("Inventory is null");
      }
    }

    void LoadAllRecipes()
    {
      ItemRecipeData[] recipeData = Utils.LoadAllFrom<ItemRecipeData>(RECIPE_DIR);
      ItemRecipe[] recipes = new ItemRecipe[recipeData.Length];
      for (int i = 0; i < recipeData.Length; ++i) {
        recipes[i] = new ItemRecipe(recipeData[i]);
      }
      foreach (var recipe in recipes) {
        var product = recipe.RecipeData.Product;
        if (!this.recipeTable.ContainsKey(product)) {
          Debug.Log($"Recipe Product {product.Name} is not loaded from {RECIPE_DIR}");
          continue;
        }
        this.recipeTable[product].Add(recipe);
        #if UNITY_EDITOR
        this.recipes.Add(recipe);
        #endif
      }

    }

    void LoadAllItems()
    {
      ItemData[] items = Utils.LoadAllFrom<ItemData>(ITEM_DIR);
      foreach (var item in items) {
        if (item.IsCraftable) {
          this.recipeTable.Add(item, new ());
        } 
      }
    }
  }
}

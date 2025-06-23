using System.Collections.Generic;
using UnityEngine;
using Patterns;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SHG
{
  public class RecipeRegistry : SingletonBehaviour<RecipeRegistry>
  {
    const string ITEM_DIR = "Assets/SHG/Test/Items";
    const string RECIPE_DIR = "Assets/SHG/Test/Recipes";
    //TODO: 아이템 레시피 최적화 자료구조
    Dictionary<ItemData, List<ItemRecipe>> recipeTable;
    #if UNITY_EDITOR
    List<ItemRecipe> recipes = new ();
    #endif

    public List<ItemRecipe> GetRecipes(ItemData craftableItem)
    {
      if (this.recipeTable.TryGetValue(craftableItem, out List<ItemRecipe> recipes)) {
        return (recipes);
      }
      return (new());
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
      ItemRecipeData[] recipeData =  this.LoadAllFrom<ItemRecipeData>(RECIPE_DIR);
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
      ItemData[] items = this.LoadAllFrom<ItemData>(ITEM_DIR);
      foreach (var item in items) {
        if (item.IsCraftable) {
          this.recipeTable.Add(item, new ());
        } 
      }
    }

    T[] LoadAllFrom<T>(in string dir) where T: UnityEngine.Object
    {
#if UNITY_EDITOR
      string[] guids = AssetDatabase.FindAssets(
        $"t:{typeof(T).Name}", new[] { dir });
      int count = guids.Length;
      if (count == 0) {
        Debug.Log($"No {typeof(T).Name} is found in {dir}");
      }
      T[] loaded = new T[count];
      for(int i = 0; i < count; i++) {
        var path = AssetDatabase.GUIDToAssetPath(guids[i]);
        loaded[i] = AssetDatabase.LoadAssetAtPath<T>(path);
      }
      return (loaded);
#else
      T[] loaded = Resources.LoadAll(dir) as T[];
      if (loaded.Length == 0) {
        Debug.Log($"No {typeof(T).Name} is found in {dir}");
      }
      return (loaded);
#endif
    }
  }

}

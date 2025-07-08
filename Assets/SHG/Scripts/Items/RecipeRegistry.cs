using System.Collections.Generic;
using UnityEngine;
using Patterns;
using System;

namespace SHG
{
  public class RecipeRegistry : SingletonBehaviour<RecipeRegistry>, IObservableObject<RecipeRegistry>
  {
    public const int NUMBER_OF_PRODUCTS = 40;
    public static readonly string[] RECIPE_DIRS = new string[5] {
      "Assets/PJW/Recipe/DropChangeRecipe",
      "Assets/PJW/Recipe/EquipmentRecipe",
      "Assets/PJW/Recipe/PlainRecipe",
      "Assets/PJW/Recipe/RecoveryRecipe",
      "Assets/PJW/Recipe/StoryRecipe"
    };
    public Dictionary<ItemData, List<ItemRecipe>>[] AvailableRecipes;
    public static readonly List<ItemRecipe> EMPTY_RECIPES = new (0);
    //TODO: 아이템 레시피 최적화 자료구조
    Dictionary<ItemData, List<ItemRecipe>> recipeTable;
    #if UNITY_EDITOR
    List<ItemRecipe> recipes = new ();

    public Action<RecipeRegistry> WillChange { get; set; }
    public Action<RecipeRegistry> OnChanged { get; set;}
#endif

    public IEnumerable<ItemData> GetAllProducts(CraftProvider provider)
    {
      return (this.AvailableRecipes[(int)provider - 1].Keys);
    }

    public List<ItemRecipe> GetRecipes(ItemData craftableItem, CraftProvider provider)
    {
      if (this.AvailableRecipes[(int)provider - 1].TryGetValue(craftableItem, out List<ItemRecipe> recipes)) {
        return (recipes.FindAll(
          recipe => (recipe.Provider == CraftProvider.All ||
          recipe.Provider == provider)));
      }
      return (EMPTY_RECIPES);
    }

    public void RegisterItemUse(Inventory inventory)
    {
      inventory.OnUseItem += this.OnUseItem;
    }

    void OnUseItem(ItemData item)
    {
      bool isChanged = false;
      if (item is DropChangeItemData dropChangeItem) {
        foreach (var recipe in dropChangeItem.UnlockedRecipesWhenUse) {
          isChanged = true;
          if (recipe.Provider == CraftProvider.All) {
            this.AddRecipe(0, recipe);
            this.AddRecipe(1, recipe);
          }
          else {
            this.AddRecipe((int)recipe.Provider - 1, recipe);
          }
        }
      }
      if (isChanged) {
        this.OnChanged?.Invoke(this);
      }
    }

    void AddRecipe(int providerIndex, ItemRecipeData recipeData)
    {
      Debug.Log($"AddRecipe: {recipeData.Product.name}");
      if (this.AvailableRecipes[providerIndex].TryGetValue(recipeData.Product, out List<ItemRecipe> list)) {
        if (list.FindIndex(recipe => recipe.RecipeData == recipeData) == -1) {
          list.Add(new ItemRecipe(recipeData));
        }
      }
      else {
        this.AvailableRecipes[providerIndex][recipeData.Product] = new List<ItemRecipe>() {
          new ItemRecipe(recipeData)
        };
      }
    }

    protected override void Awake()
    {
      base.Awake();
      this.Init();
    }

    void Init()
    {
      this.AvailableRecipes = new Dictionary<ItemData, List<ItemRecipe>>[2] { 
        new(), new ()
      };
      this.recipeTable = new ();
      this.LoadAllItems();
      this.LoadAllRecipes();
    }

    void LoadAllRecipes()
    {
      foreach (var recipeDir in RECIPE_DIRS) {

        ItemRecipeData[] recipeData = Utils.LoadAllFrom<ItemRecipeData>(recipeDir);
        ItemRecipe[] recipes = new ItemRecipe[recipeData.Length];
        for (int i = 0; i < recipeData.Length; ++i) {
          recipes[i] = new ItemRecipe(recipeData[i]);
        }
        foreach (var recipe in recipes) {
          var product = recipe.RecipeData.Product;
          if (!this.recipeTable.ContainsKey(product)) {
            Debug.Log($"Recipe Product {product.Name} is not loaded from {recipeDir}");
            continue;
          }
          this.recipeTable[product].Add(recipe);
          if (!recipe.RecipeData.IsLock) {
            var index = (int)recipe.Provider - 1;
            if (this.AvailableRecipes[index].TryGetValue(recipe.RecipeData.Product, out List<ItemRecipe> list)) {
              list.Add(recipe);
            }
            else {
              this.AvailableRecipes[index][recipe.RecipeData.Product] = new List<ItemRecipe> () { recipe };
            }
          }
#if UNITY_EDITOR
          this.recipes.Add(recipe);
#endif
        }
      }
    }

    void LoadAllItems()
    {

      //var enumerator = ItemDatabase.GetEnumerator();
      //while (enumerator.MoveNext()) {
      //  var item = enumerator.Current.Value;
      //  if (item.IsCraftable) {
      //    this.recipeTable.Add(item, new ());
      //  } 
      //}
      foreach (var item in ItemStorageBase.ALL_ITEMS) {
        if (item.IsCraftable) {
          this.recipeTable.Add(item, new ());
        } 
      }
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemRegistry : SingletonBehaviour<ItemRegistry>
{
  const string ITEM_DIR = "Assets/SHG/Test/Items";
  const string RECIPE_DIR = "Assets/SHG/Test/Recipes";
  public List<ItemData> AllItemData { get; private set; }

  protected override void Awake()
  {
    base.Awake();
    this.AllItemData = new ();
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
  }

  void LoadAllItems()
  {
    var items = this.LoadAllFrom<ItemData>(ITEM_DIR);
    foreach (var item in items) {
      Debug.Log($"{item.Name}"); 
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

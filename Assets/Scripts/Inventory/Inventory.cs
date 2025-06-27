using System;
using System.Collections.Generic;
using KSH;
using UnityEngine;
using Patterns;
using SHG;

public class Inventory : SingletonBehaviour<Inventory>, IObservableObject<Inventory>
{

  public const int QUICKSLOT_COUNT = 4;
  public const int MAX_STACK_COUNT = 20;
  public const int MAX_SLOT_COUNT = 20;
  public Dictionary<ItemData, int> Items { get; private set; }
  public Action<Inventory> WillChange { get; set; }
  public Action<Inventory> OnChanged { get; set; }
  public List<ItemData> QuickSlotItems { get; private set;}

#if UNITY_EDITOR
  [SerializeField]
  public List<string> ItemNamesForDebugging;
  [SerializeField]
  public List<string> ItemNamesForDebuggingInQuickSlot;
#endif

  public ItemData[] PeakItemsInQuickSlot()
  {
    var items = new ItemData[this.QuickSlotItems.Count];
    this.QuickSlotItems.CopyTo(items);
    return (items);
  }

  public bool IsAbleToAddItem(ItemAndCount itemAndCount)
  {
    var currentSlotCount = this.CountUsedSlot();
    if (currentSlotCount < MAX_SLOT_COUNT) {
      return (true);
    }
    if (currentSlotCount == MAX_SLOT_COUNT) {
      var currentItemCount = this.GetItemCount(itemAndCount.Item);
      if ((currentItemCount % MAX_STACK_COUNT) + itemAndCount.Count <
        MAX_STACK_COUNT) {
        return (true);
      }
    }
    return (false);
  }

  public int CountUsedSlot()
  {
    int count = 0;   
    foreach (var itemAndCount in this.Items) {
      var itemCount = itemAndCount.Value;
      if (itemCount % MAX_STACK_COUNT == 0) {
        count += (itemCount / MAX_STACK_COUNT);
      }
      else {
        count += (itemCount / MAX_STACK_COUNT) + 1;
      }
    }
    return (count);
  }

  public void MoveItemToQuickSlot(ItemData itemData)
  {
    if (itemData is EquipmentItemData equipmentItemData) {
      var currentCount = this.GetItemCount(itemData);
      if (currentCount > 0) {
        this.WillChange?.Invoke(this);
        this.QuickSlotItems.Add(equipmentItemData);
        if (currentCount > 1) {
          this.Items[itemData] = currentCount - 1;
        }
        else {
          this.Items.Remove(itemData);
        }
        #if UNITY_EDITOR
        this.RemoveItemName(itemData);
        this.AddQuickSlotItemName(itemData);
        #endif
        this.OnChanged?.Invoke(this);
      }
      else {
        throw (new ApplicationException($"{itemData.Name} is not found in Inventory"));
      }
    }
    else {
      throw (new ArgumentException($"{itemData.Name} is not equipment item, invalid move to quickslot"));
    }
  }

  public void UseItem(IUsable item)
  {
    if (item is RecoveryItem recoveryItem)
    {
      var recoveryItemData = recoveryItem.Recovery();

      foreach (var data in recoveryItemData)
      {
        switch (data.Stat)
        {
          case TempCharacter.Stat.Hp:
            PlayerStatManager.Instance.HP.Heal(data.Amount);
            break;
          case TempCharacter.Stat.Hydration:
            PlayerStatManager.Instance.Thirsty.Drink(data.Amount);
            break;
          case TempCharacter.Stat.Hunger:
            PlayerStatManager.Instance.Hunger.Eat(data.Amount);
            break;
          case TempCharacter.Stat.Fatigue:
            PlayerStatManager.Instance.Fatigue.Sleep(data.Amount);
            break;
        }
      }
    }
    else {
      throw (new NotImplementedException());
    }
  }

  public void MoveItemFromQuickSlot(ItemData itemData)
  {
    if (itemData is EquipmentItemData equipmentItemData) {
      var index = this.QuickSlotItems.IndexOf(equipmentItemData);
      if (index != -1) {
        this.WillChange?.Invoke(this);
        this.QuickSlotItems.RemoveAt(index);
        if (this.Items.TryGetValue(itemData, out int currentCount)) {
          this.Items[itemData] = currentCount + 1;
        }
        else {
          this.Items.Add(itemData, 1);
        }
        #if UNITY_EDITOR
        this.AddItemName(itemData);
        this.RemoveQuickSlotItemName(itemData);
        #endif
        this.OnChanged?.Invoke(this);
      }
      else {
        throw (new ApplicationException($"{itemData.Name} is not found in Inventory"));
      }
    }
    else {
      throw (new ArgumentException($"{itemData.Name} is not equipment item, invalid move to quickslot"));
    }

  }

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
    this.WillChange?.Invoke(this);
    foreach (var required in recipe.RequiredItems) {
      if (!this.Items.TryGetValue(required.Item, out int count) ||
        count < required.Count) {
        throw (new ApplicationException($"not enough material for {recipe.RecipeData.Product}"));
      }  
      else {
        this.Items[required.Item] = count - required.Count;
#if UNITY_EDITOR
        for (int i = 0; i < required.Count; i++) {
          this.RemoveItemName(required.Item);    
        }
#endif
      }
    } 
    this.OnChanged?.Invoke(this);
    return (Item.CreateItemFrom(recipe.RecipeData.Product));
  }

  public void AddItem(Item item)
  {
    this.WillChange?.Invoke(this);
#if UNITY_EDITOR
    this.AddItemName(item.Data);
    if (!this.IsAbleToAddItem(new ItemAndCount { Item = item.Data, Count = 1})) {
      throw (new ApplicationException($"Unable to add more {item.Data.Name}"));
    }
#endif
    if (this.Items.TryGetValue(item.Data, out int itemCount)) {
      this.Items[item.Data] = itemCount + 1;
    }
    else {
      this.Items.Add(item.Data, 1);
    }
    this.OnChanged?.Invoke(this);
  }

  public int GetItemCouontInQuickSlot(ItemData itemData)
  {
    int count = 0;
    for (int i = 0; i < this.QuickSlotItems.Count; i++) {
      if (this.QuickSlotItems[i] == itemData) {
        count += 1;
      } 
    }
    return (count);
  }

#if UNITY_EDITOR
  void AddItemName(ItemData data) 
  {
    this.ItemNamesForDebugging.Add(data.Name);
    this.ItemNamesForDebugging.Sort();
  }
#endif

#if UNITY_EDITOR
  void AddQuickSlotItemName(ItemData data) 
  {
    this.ItemNamesForDebuggingInQuickSlot.Add(data.Name);
    this.ItemNamesForDebuggingInQuickSlot.Sort();
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

#if UNITY_EDITOR
  void RemoveItemName(ItemData itemData)
  {
    var index = this.ItemNamesForDebugging.FindIndex(
      name => name == itemData.Name);
    if (index != -1) {
      this.ItemNamesForDebugging.RemoveAt(index);
    }
  }
#endif

#if UNITY_EDITOR
  void RemoveQuickSlotItemName(ItemData itemData)
  {
    var index = this.ItemNamesForDebuggingInQuickSlot.FindIndex(
      name => name == itemData.Name);
    if (index != -1) {
      this.ItemNamesForDebuggingInQuickSlot.RemoveAt(index);
    }
  }
#endif

  public Item GetItem(ItemData itemData)
  {
    int itemCount = this.GetItemCount(itemData);
    if (itemCount < 1) {
      throw (new ApplicationException($"GetItem: No {itemData.Name} in Inventory")); 
    }
    this.WillChange?.Invoke(this);
    Item item = Item.CreateItemFrom(itemData);
#if UNITY_EDITOR
    this.RemoveItemName(itemData);
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

  public Item PeakItem(ItemData itemData)
  {
    int itemCount = this.GetItemCount(itemData);
    if (itemCount < 1) {
      throw (new ApplicationException($"GetItem: No {itemData.Name} in Inventory")); 
    }

    return (Item.CreateItemFrom(itemData));
  }

  protected override void Awake()
  {
    base.Awake();
    this.Items = new ();
    this.QuickSlotItems = new ();
#if UNITY_EDITOR
    this.ItemNamesForDebugging = new();
    this.ItemNamesForDebuggingInQuickSlot = new();
#endif
  }
}

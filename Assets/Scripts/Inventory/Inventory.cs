using System;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using KSH;
using SHG;
using NTJ;

public class Inventory : ItemStorageBase
{
  public const string BAG_ID = "8793a64f-c4e3-41a4-b242-21cd4d7f4f49";
  public const string RADIO_ID = "b5778b62-1b8b-4000-aba5-14992b4348ea";
  public const int QUICKSLOT_COUNT = 4;
  public List<ItemData> QuickSlotItems { get; private set;}
  public Action<ItemData> OnUseItem;
  public Action<DropChangeItem> OnDropChangeItemUsed;
  public override int MAX_SLOT_COUNT => this.slotCount;
  int slotCount = 10;

#if UNITY_EDITOR
  [SerializeField]
  public List<string> ItemNamesForDebuggingInQuickSlot;
#endif

  public EquipmentItem GetItemFromQuickSlot(EquipmentItemData item)
  {
    int index = this.QuickSlotItems.IndexOf(item);
    if (index == -1) {
      throw (new ApplicationException($"no {item.Name} in quickslot"));
    }
    this.WillChange?.Invoke(this);
    this.QuickSlotItems.RemoveAt(index);
    this.OnChanged.Invoke(this);
    return (Item.CreateItemFrom(item) as EquipmentItem);
  }

  public Inventory(): base()
  {
    this.QuickSlotItems = new ();
    this.ItemNamesForDebuggingInQuickSlot = new();
  }

  public ItemData[] PeakItemsInQuickSlot()
  {
    var items = new ItemData[this.QuickSlotItems.Count];
    this.QuickSlotItems.CopyTo(items);
    return (items);
  }

  public List<string> GetQuickSlotItemIDs()
  {
    return (this.QuickSlotItems.ConvertAll(item => item.Id));
  }

  public void RegisterEventRewards(GameEventHandler eventHandler)
  {
    eventHandler.OnNormalEventStart += this.OnEventStart;
    eventHandler.OnStoryEventStart += this.OnEventStart;
  }

  void OnEventStart(GameEvent gameEvent)
  {
    for (int i = 0; i < gameEvent.Rewards.Length; i++) {
      if (gameEvent.Rewards[i] != null && gameEvent.Rewards[i] is ItemReward itemReward) {
        if (itemReward.IsLost) {
          this.LoseItems(itemReward.Items);
        }       
        else {
          foreach (var item in itemReward.Items) {
            this.AddItem(Item.CreateItemFrom(item)); 
          }
        }
      } 
    }
  }

  void LoseItems(ItemData[] items)
  {
    foreach (var item in items) {
      if (this.GetItemCount(item) > 1) {
        this.GetItem(item);
      } 
    }
  }

  public void LoadQuickSlotItems(List<string> itemIds)
  {
    for (int i = 0; i < itemIds.Count; i++) {
      var item = ItemDatabase.GetItemDataByID(itemIds[i]);
      if (item != null) {
        this.QuickSlotItems[i] = item;
      }
      #if UNITY_EDITOR
      else {
        Debug.LogError($"Fail to get item from {nameof(ItemDatabase)} by id: {itemIds[i]}");
      }
      #endif
    }    
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
      if (recoveryItem.Data.Id == BAG_ID) {
        this.slotCount += 5;
        this.OnChanged?.Invoke(this);
      }
      else {
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
    }
    else if (item is DropChangeItem dropChangeItem) {
      this.OnDropChangeItemUsed?.Invoke(dropChangeItem);
    }
    else {
      throw (new NotImplementedException());
    }
    if (item is Item itemClass) {
      this.OnUseItem?.Invoke(itemClass.Data);
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

  public List<ItemRecipe> GetCraftableRecipes(ItemData product, CraftProvider provider)
  {
    if (product.Recipes.Length == 0) {
      return (RecipeRegistry.EMPTY_RECIPES);
    }
    var recipes = RecipeRegistry.Instance.GetRecipes(product, provider);
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
    void AddQuickSlotItemName(ItemData data) 
    {
      this.ItemNamesForDebuggingInQuickSlot.Add(data.Name);
      this.ItemNamesForDebuggingInQuickSlot.Sort();
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


#if UNITY_EDITOR
  void AddItemName(ItemData data) 
  {
    this.ItemNamesForDebugging.Add(data.Name);
    this.ItemNamesForDebugging.Sort();
  }
#endif

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
}

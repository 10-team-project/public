using System;
using EditorAttributes;
using UnityEngine;
using Void = EditorAttributes.Void;

namespace SHG
{
  [Serializable]
  public class ItemDropChange
  {
    public enum DropTableChange
    {
      AddToTable,
      RemoveFromTable
    } 
    public DropTableChange TableChange;
    public ItemData Item;
  }

  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Items/Drop Change Item")]
  public class DropChangeItemData : ItemData
  {
    public ItemDropChange[] OnObtain => this.onObtainChange;
    public ItemDropChange[] OnUse => this.onUseChange;
    public ItemRecipe[] UnlockedRecipesWhenUse => this.unlockedRecipes;

    [SerializeField]
    [Validate("Item to change is none", nameof(IsNullObtainChangeItem), MessageMode.Error)]
    ItemDropChange[] onObtainChange;
    [SerializeField]
    [Validate("Item to change is none", nameof(IsNullUseChangeItem), MessageMode.Error)]
    ItemDropChange[] onUseChange;
    [SerializeField, ReadOnly]
    [Validate("No item drop is change", nameof(IsItemDropChangeNull), MessageMode.Error)]
    Void emptyChangeCheck;
    [SerializeField] [Validate("Some unlock recipe is none", nameof(HasNullUnlockRecipe), MessageMode.Error)]
    ItemRecipe[] unlockedRecipes;

    protected bool HasNullUnlockRecipe()
    {
      if (this.unlockedRecipes == null ||
        this.unlockedRecipes.Length == 0) {
        return (false);
      }
      for (int i = 0; i < this.unlockedRecipes.Length; i++) {
        if (this.unlockedRecipes[i] == null) {
          return (true);
        } 
      }
      return (false);
    }

    protected bool IsNullUseChangeItem()
    {
      if (this.OnUse == null || this.OnUse.Length == 0) {
        return (false);
      }
      for (int i = 0; i < this.OnUse.Length; i++) {
        if (this.OnUse[i].Item == null) {
          return (true);
        } 
      }
      return (false);
    }

    protected bool IsNullObtainChangeItem()
    {
      if (this.OnObtain == null || this.OnObtain.Length == 0) {
        return (false);
      }
      for (int i = 0; i < this.OnObtain.Length; i++) {
        if (this.OnObtain[i].Item == null) {
          return (true);
        } 
      }
      return (false);
    }

    protected bool IsItemDropChangeNull => (
      (this.OnUse == null || this.OnUse.Length == 0) &&
      (this.OnObtain == null || this.OnObtain.Length == 0)
      ) ;
  }
}

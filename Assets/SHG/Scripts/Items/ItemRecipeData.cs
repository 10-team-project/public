using System;
using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Items/ItemRecipe")]
  public class ItemRecipeData : IdentifiableScriptableObject
  {
    public ItemData Product => this.product;
    public ItemData[] Materials => this.materials;
    public ItemData[] UnlockItemByUse => this.itemsToUseForUnlock;

    [SerializeField, Required]
    protected ItemData product;
    [SerializeField, Validate("Material is none", nameof(HasNullMaterial), MessageMode.Error)]
    protected ItemData[] materials;
    [SerializeField, ReadOnly, Validate("Materials are empty", nameof(IsMaterialEmpty), MessageMode.Error)] 
    protected Void emptyMaterialError;
    [Validate("Unlock item is empty", nameof(IsUnlockItemEmpty), MessageMode.Warning)]
    protected ItemData[] itemsToUseForUnlock;
    [Validate("Some unlock item is none", nameof(HasInvalidUnlockItem), MessageMode.Error)]
    protected Void invalidUnlockItemCheck;

    protected bool IsMaterialEmpty() => (this.Materials == null || this.Materials.Length == 0);

    protected bool HasNullMaterial()
    {
      if (this.IsMaterialEmpty()) {
        return (false);
      }
      foreach (var material in this.Materials) {
        if (material == null) {
          return (true);
        } 
      }
      return (false);
    }

    protected bool IsUnlockItemEmpty() => (this.UnlockItemByUse == null || this.UnlockItemByUse.Length == 0);

    protected bool HasInvalidUnlockItem()
    {
      if (this.IsUnlockItemEmpty()) {
        return (false);
      }
      for (int i = 0; i < this.UnlockItemByUse.Length; i++) {
        if (this.UnlockItemByUse[i] == null) {
          return (true);
        } 
      }
      return (false);
    }
  }
}

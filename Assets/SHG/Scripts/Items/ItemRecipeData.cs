using System;
using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Items/ItemRecipe")]
  public class ItemRecipeData : ScriptableObject
  {
    public ItemData Product => this.product;
    public ItemData[] Materials => this.materials;

    [SerializeField, Required]
    protected ItemData product;
    [SerializeField, Validate("Material is none", nameof(hasNullMaterial), MessageMode.Error)]
    protected ItemData[] materials;
    [SerializeField, ReadOnly, Validate("Materials are empty", nameof(IsMaterialEmpty), MessageMode.Error)] 
    protected Void emptyMaterialError;

    protected bool IsMaterialEmpty() => (this.Materials == null || this.Materials.Length == 0);
    protected bool hasNullMaterial()
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
  }
}

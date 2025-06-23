using System;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Items/ItemRecipe")]
  public class ItemRecipeData : ScriptableObject
  {
    public CraftableItemData Product => this.product;
    public ItemData[] Materials => this.materials;

    [SerializeField, Required]
    protected CraftableItemData product;
    [SerializeField, Validate("Materials are empty")] 
    protected ItemData[] materials;

    protected bool IsMaterialEmpty() => (this.Materials == null || this.Materials.Length == 0);
  }
}

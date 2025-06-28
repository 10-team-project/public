using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

namespace SHG
{
  public abstract class MapObjectData : ScriptableObject
  {
    public EquipmentItemData[] RequiredItems => this.requiredItems;

    protected Void emptyMaterialError;
    public string Description => this.description;
    [HideInInspector]
    public GameObject Prefab => this.prefab;
    [SerializeField]
    string description;
    [SerializeField] [Validate("Some required item is none", nameof(HasNullRequiredItems), MessageMode.Error)]
    EquipmentItemData[] requiredItems;
    [SerializeField, AssetPreview(64f, 64f), Validate("Prefab is none", nameof(IsPrefabNone), MessageMode.Warning)]
    protected GameObject prefab;

    [SerializeField, ReadOnly, Validate("RequiredItems are empty", nameof(IsRequiredItemEmpty), MessageMode.Warning)] 
    protected Void emptyRequiredCheck;
    protected bool IsPrefabNone() => this.Prefab == null;
    protected bool IsRequiredItemEmpty()
    {
      return (this.requiredItems == null || this.requiredItems.Length == 0);
    }

    protected bool HasNullRequiredItems()
    {
      if (this.IsRequiredItemEmpty()) {
        return (false);
      }
      foreach (var item in this.RequiredItems) {
        if (item == null) {
          return (true);
        } 
      }
      return (false);
    }
  }
}

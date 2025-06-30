using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

namespace SHG
{
  public abstract class MapObjectData : IdentifiableScriptableObject
  {
    public EquipmentItemData[] RequiredItems => this.requiredItems;

    public string Name => this.objectName;
    public string Description => this.description;

    [HideInInspector]
    public GameObject Prefab => this.prefab;
    [SerializeField] [Validate("object name is none", nameof(IsObejectNameEmpty), MessageMode.Error)]
    string objectName;
    [SerializeField]
    string description;
    [SerializeField] [Validate("Some required item is none", nameof(HasNullRequiredItems), MessageMode.Error)]
    EquipmentItemData[] requiredItems;
    [SerializeField, AssetPreview(64f, 64f), Validate("Prefab is none", nameof(IsPrefabNone), MessageMode.Error)]
    protected GameObject prefab;

    [SerializeField, ReadOnly, Validate("RequiredItems are empty", nameof(IsRequiredItemEmpty), MessageMode.Warning)] 
    protected Void emptyRequiredCheck;
    protected bool IsPrefabNone() => this.Prefab == null;
    protected bool IsRequiredItemEmpty()
    {
      return (this.RequiredItems == null || this.RequiredItems.Length == 0);
    }

    protected bool IsObejectNameEmpty()
    {
      return (this.objectName == null || this.objectName.Replace(" ", "").Length == 0);
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

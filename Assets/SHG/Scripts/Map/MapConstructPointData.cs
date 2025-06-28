using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

namespace SHG
{
  [CreateAssetMenu (menuName = "ScriptableObjects/Map/Construrct point")]
  public class MapConstructPointData : MapObjectData
  {
    [SerializeField, ReadOnly, Validate("some required item is not for construct", nameof(HasInvalidRequiredItem), MessageMode.Error)] 
    protected Void invalidRequiredItemCheck;

    protected bool HasInvalidRequiredItem() {
      if (this.IsRequiredItemEmpty()) {
        return (false);
      }
      foreach (var item in this.RequiredItems) {
        if (item.Purpose == EquipmentItemPurpose.Destruct) {
          return (true);
        }     
      }
      return (false);
    }
  }
}


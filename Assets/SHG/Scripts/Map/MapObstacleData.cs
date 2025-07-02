using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

namespace SHG
{
  [CreateAssetMenu (menuName = "ScriptableObjects/Map/Obstacle")]
  public class MapObstacleData : MapObjectData
  {
    [SerializeField, ReadOnly, Validate("some required item is not for obstacle", nameof(HasInvalidRequiredItem), MessageMode.Error)] 
    protected Void invalidRequiredItemCheck;

    protected bool HasInvalidRequiredItem() {
      if (this.IsRequiredItemEmpty()) {
        return (false);
      }
      foreach (var item in this.RequiredItems) {
        if (item == null) {
          return (false);
        }
        if (item.Purpose == EquipmentItemPurpose.Construct) {
          return (true);
        }     
      }
      return (false);
    }
  }
}

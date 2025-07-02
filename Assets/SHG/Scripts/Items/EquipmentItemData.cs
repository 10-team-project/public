using UnityEngine;

namespace SHG
{

  [CreateAssetMenu (menuName = "ScriptableObjects/Items/Equipment Item")]
  public class EquipmentItemData : ItemData
  {
    public EquipmentItemPurpose Purpose => this.usePurpose;

    [SerializeField]
    EquipmentItemPurpose usePurpose;
  }
}

using UnityEngine;

namespace SHG
{

  [CreateAssetMenu (menuName = "ScriptableObjects/Items/Equipment Item")]
  public class EquipmentItemData : ItemData
  {
    [HideInInspector]
    public bool IsWeapon => this.isWeapon;
    [HideInInspector]
    public MountingType Mounting => this.mounting;

    [SerializeField]
    bool isWeapon;
    [SerializeField]
    MountingType mounting;
  }
}

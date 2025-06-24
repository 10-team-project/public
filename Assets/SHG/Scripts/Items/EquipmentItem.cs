
namespace SHG
{
  public enum MountingType
  {
    SingleHand,
    BothHands,
    SingleOrBothHand
  }

  public class EquipmentItem : Item
  {
    public bool IsWeapon => ((EquipmentItemData)(this.Data)).IsWeapon;
    public MountingType Mounting => ((EquipmentItemData)(this.Data)).Mounting;
    public EquipmentItem(EquipmentItemData data) : base(data) { }
  }
}

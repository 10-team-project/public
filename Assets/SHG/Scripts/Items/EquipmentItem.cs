
namespace SHG
{
  public enum EquipmentItemPurpose
  {
    Construct,
    Destruct
  }

  public class EquipmentItem : Item
  {
    public EquipmentItemPurpose Purpose { get; private set; }
    public EquipmentItem(EquipmentItemData data) : base(data) { 
      this.Purpose = data.Purpose;
    }
  }
}

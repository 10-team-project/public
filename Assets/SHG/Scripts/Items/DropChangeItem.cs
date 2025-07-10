using System;

namespace SHG
{
  public class DropChangeItem : Item, IUsable
  {
    public ItemDropChange[] OnObtain { get; private set; }
    public ItemDropChange[] OnUse { get; private set; }

    public Action<IUsable> OnUsed { get; set; }

    public DropChangeItem(DropChangeItemData data) : base(data)
    {
      this.OnObtain = data.OnObtain;
      this.OnUse = data.OnUse;
    }
  }
}

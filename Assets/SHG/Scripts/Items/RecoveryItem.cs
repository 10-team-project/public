using System;

namespace SHG
{
  public partial class RecoveryItem : Item, IUsable, IRecoveryable
  {
    RecoveryItemData recoveryItemData;
    public Action<IUsable> OnUsed { get; set; }

    public RecoveryItem(RecoveryItemData data): base(data)
    {
      this.recoveryItemData = data;
    }

    public Efficacy[] Recovery()
    {
      this.OnUsed?.Invoke(this);
      return (recoveryItemData.Efficacies);
    }
  }
}

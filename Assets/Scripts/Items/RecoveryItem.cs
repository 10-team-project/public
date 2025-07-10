using System;

public partial class RecoveryItem : Item, IUsable, IRecoveryable
{
  public Action<IUsable> OnUsed { get; set; }

  public Efficacy[] Efficacies => ((RecoveryItemData)this.Data).Efficacies;

  public RecoveryItem(RecoveryItemData data): base(data) {}

  public Efficacy[] Recovery()
  {
    this.OnUsed?.Invoke(this);
    return (((RecoveryItemData)this.Data).Efficacies);
  }
}

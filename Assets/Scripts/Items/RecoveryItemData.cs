using System;
using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

[Serializable]
[CreateAssetMenu (menuName = "ScriptableObjects/Items/Recovery Item")]
public class RecoveryItemData : ItemData
{
  [SerializeField, ReadOnly, Validate("Must have some efficacy", nameof(this.EmptyEfficacy), MessageMode.Error)]
  Void emptyEfficacyCheck;
  [SerializeField, ReadOnly, Validate("Amount must greater than zero", nameof(this.HasInvalidAmount), MessageMode.Warning)]
  Void amountCheck;
  public Efficacy[] Efficacies => this.efficacy;
  [SerializeField]
  Efficacy[] efficacy;

  protected bool EmptyEfficacy() => this.efficacy == null || this.Efficacies.Length == 0;

  protected bool HasInvalidAmount() {
    if (this.EmptyEfficacy()) {
      return (false);
    }
    foreach (var efficacy in this.Efficacies) {
      if (efficacy.Amount <= 0) {
        return (true);
      } 
    }
    return (false);
  }
}

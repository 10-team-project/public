using System;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Date Trigger")]
  public class DateChangeTrigger : GameEventTrigger
  {
    public int Date => this.triggerDate;
    [Validate("Invalid date", nameof(IsInvalidDate), MessageMode.Error)]
    int triggerDate;

    protected bool IsInvalidDate() => (this.triggerDate < 0 || this.triggerDate > 1000);
  }
}

using System;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Item Trigger")]
  public class NewItemTrigger : GameEventTrigger
  {
    public ItemData Item => this.triggerItem;
    [SerializeField] [Validate("Trigger item is none", nameof(IsNullTriggerItem), MessageMode.Error)]
    ItemData triggerItem;

    protected bool IsNullTriggerItem() => (this.triggerItem == null);
  }
}

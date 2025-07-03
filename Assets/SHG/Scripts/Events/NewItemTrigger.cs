using System;
using UnityEngine;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Item Trigger")]
  public class NewItemTrigger : GameEventTrigger
  {
    public ItemData Item => this.triggerItem;
    ItemData triggerItem;
  }
}

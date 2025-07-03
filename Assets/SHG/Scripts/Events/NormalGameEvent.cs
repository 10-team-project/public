using System;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Normal Event")]
  public class NormalGameEvent : GameEvent
  {
    public GameEventTrigger Trigger => this.eventTrigger;
    [Validate("Event trigger is none", nameof(IsNullTrigger), MessageMode.Error)]
    GameEventTrigger eventTrigger;

    protected bool IsNullTrigger() => (this.Trigger == null);
  }
}

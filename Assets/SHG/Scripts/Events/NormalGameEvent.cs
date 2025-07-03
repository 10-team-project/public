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
    public int Priority => this.eventPriority;
    [SerializeField]
    [Validate("Event trigger is none", nameof(IsNullTrigger), MessageMode.Error)]
    GameEventTrigger eventTrigger;
    [SerializeField]
    [Range(1, 100)] [Validate("priority range is 1 to 100", nameof(IsInvalidPriority), MessageMode.Error)]
    int eventPriority;

    protected bool IsNullTrigger() => (this.Trigger == null);

    protected bool IsInvalidPriority => (this.Priority < 1 || this.Priority > 100);
  }
}

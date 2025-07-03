using System;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Story Event")]
  public class StoryGameEvent : GameEvent
  {
    public GameEventTrigger Trigger => this.eventTrigger;
    public int Priority => this.eventPriority;
    public AudioClip Sound => this.effectSound;
    [SerializeField]
    [Validate("Event trigger is none", nameof(IsNullTrigger), MessageMode.Error)]
    GameEventTrigger eventTrigger;
    [SerializeField]
    [Range(1, 100)] [Validate("priority range is 1 to 100", nameof(IsInvalidPriority), MessageMode.Error)]
    int eventPriority;
    [SerializeField, AssetPreview(64f, 64f)]
    AudioClip effectSound;

    protected bool IsNullTrigger() => (this.Trigger == null);

    protected bool IsInvalidPriority => (this.Priority < 1 || this.Priority > 100);
  }
}

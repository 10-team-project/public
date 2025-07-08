using System;
using UnityEngine;
using EditorAttributes;
using LTH;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Story Event")]
  public class StoryGameEvent : GameEvent
  {
    public override bool IsStoryEvent => true;
    public GameEventTrigger Trigger => this.eventTrigger;
    public int Priority => this.eventPriority;
    public AudioClip Sound => this.effectSound;
    public SceneTraumaTransition Trauma => this.traumaEffect;
    [SerializeField]
    [Validate("Event trigger is none", nameof(IsNullTrigger), MessageMode.Error)]
    GameEventTrigger eventTrigger;
    [SerializeField]
    [Range(1, 100)] [Validate("priority range is 1 to 100", nameof(IsInvalidPriority), MessageMode.Error)]
    int eventPriority;
    [SerializeField, AssetPreview(64f, 64f)]
    AudioClip effectSound;
    [SerializeField, AssetPreview] 
    SceneTraumaTransition traumaEffect;

    protected bool IsNullTrigger() => (this.Trigger == null);

    protected bool IsInvalidPriority => (this.Priority < 1 || this.Priority > 100);

    }
}

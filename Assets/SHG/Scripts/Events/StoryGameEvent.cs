using System;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Story Event")]
  public class StoryGameEvent : GameEvent
  {
    public AudioClip Sound => this.effectSound;
    [SerializeField, AssetPreview(64f, 64f)] AudioClip effectSound;
  }
}

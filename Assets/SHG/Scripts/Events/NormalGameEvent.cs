using System;
using UnityEngine;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Normal Event")]
  public class NormalGameEvent : GameEvent
  {
    public bool Repeatable => this.repeat;
    public override bool IsStoryEvent => false;
    [SerializeField]
    bool repeat;
  }
}

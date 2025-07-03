using System;
using UnityEngine;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Normal Event")]
  public class NormalGameEvent : GameEvent
  {
    public bool Repeatable => this.repeat;
    [SerializeField]
    bool repeat;
  }
}

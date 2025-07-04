using System;
using UnityEngine;
using Character = SHG.TempCharacter;

namespace SHG
{
  public enum ChangeTrend
  {
    Increase,
    Decrease
  }

  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Resource Trigger")]
  public class ResourceChangeTrigger : GameEventTrigger
  {
    public Character.Stat Stat => this.statType;
    public ChangeTrend Trend => this.changeTrend;
    public float Value => this.statValue;
    [SerializeField]
    Character.Stat statType;
    [SerializeField]
    ChangeTrend changeTrend;
    [SerializeField]
    float statValue;
  }
}

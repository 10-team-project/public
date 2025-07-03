using System;
using UnityEngine;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Item Reward")]
  public class ItemReward : GameEventReward
  {
    public ItemData[] Items => this.rewardItems;
    ItemData[] rewardItems;
  }
}

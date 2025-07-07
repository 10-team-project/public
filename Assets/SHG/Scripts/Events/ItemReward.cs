using System;
using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Item Reward")]
  public class ItemReward : GameEventReward
  {
    public ItemData[] Items => this.rewardItems;
    public bool IsLost => this.isLost;
    [SerializeField] [Validate("Some reward item is none", nameof(HasNullReward), MessageMode.Error)]
    ItemData[] rewardItems;
    [SerializeField, ReadOnly]
    [Validate("Item reward is empty", nameof(IsEmptyRewards), MessageMode.Warning)]
    Void nullRewardItemCheck;
    [SerializeField]
    bool isLost;

    protected bool IsEmptyRewards() => (this.rewardItems == null || this.rewardItems.Length == 0);

    protected bool HasNullReward()
    {
      if (this.IsEmptyRewards()) {
        return (false);
      }
      for (int i = 0; i < this.Items.Length; i++) {
        if (this.Items[i] == null) {
          return (true);
        } 
      }
      return (false);
    }
  }
}

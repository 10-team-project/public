using UnityEngine;

namespace SHG
{
  [CreateAssetMenu (menuName = "ScriptableObjects/Items/Story Item")]
  public class StoryItemData: ItemData
  {
    public override bool IsStoryItem => true;
  }
}

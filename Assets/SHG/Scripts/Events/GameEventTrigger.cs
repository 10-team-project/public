using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public abstract class GameEventTrigger : ScriptableObject
  {
    public float Percentage => this.chance;
    [SerializeField]
    [Range(0f, 1f)] [Validate("Invalid chance", nameof(IsInvalidChance), MessageMode.Warning)]
    float chance = 0.5f;

    protected bool IsInvalidChance() => (this.chance < 0f || this.chance > 1f);
  }
}

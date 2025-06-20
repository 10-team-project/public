using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public abstract class ItemData : ScriptableObject
  {
    [HideInInspector]
    public string Name => this.itemName;
    [HideInInspector]
    public Sprite Image => this.image;

    [SerializeField, Validate("Empty item name", nameof(IsNameEmpty), MessageMode.Error, buildKiller: true)]
    protected string itemName;
    [SerializeField, Required, AssetPreview(64f, 64f)]
    protected Sprite image;

    protected bool IsNameEmpty() => itemName.Length == 0;
  }
}

using System;
using UnityEngine;
using EditorAttributes;

[Serializable]
public abstract partial class ItemData : ScriptableObject
{
  [HideInInspector]
  public string Name => this.itemName;
  [HideInInspector]
  public Sprite Image => this.image;
  [HideInInspector]
  public GameObject Prefab => this.prefab;
  [HideInInspector]
  public string Description => this.description;

  [SerializeField, Validate("Empty item name", nameof(IsNameEmpty), MessageMode.Error, buildKiller: true)]
  protected string itemName;
  [SerializeField, AssetPreview(64f, 64f), Validate("Image is none", nameof(IsImageNone), MessageMode.Warning)]
  protected Sprite image;
  [SerializeField, AssetPreview(64f, 64f), Validate("Prefab is none", nameof(IsPrefabNone), MessageMode.Warning)]
  protected GameObject prefab;
  [SerializeField, TextArea, Validate("Description is empty", nameof(IsDescriptionEmpty), MessageMode.Warning)]
  string description;

  protected bool IsNameEmpty() => this.itemName == null || this.itemName.Replace(" ", "").Length == 0;
  protected bool IsImageNone() => this.Image == null;
  protected bool IsPrefabNone() => this.Prefab == null;
  protected bool IsDescriptionEmpty() => this.description == null ||
    this.description.Replace(" ", "").Length == 0;
}

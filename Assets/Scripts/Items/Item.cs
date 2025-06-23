using System;
using UnityEngine;

[Serializable]
public abstract partial class Item 
{
  public ItemData Data => this.data; 
  public bool IsCraftable => this.data.IsCraftable;
  [SerializeField]
  ItemData data;

  public Item(ItemData data)
  {
    this.data = data;
  }
}

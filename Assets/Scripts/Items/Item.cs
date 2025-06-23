using System;
using UnityEngine;

[Serializable]
public abstract partial class Item 
{
  public ItemData Data => this.data; 
  [SerializeField]
  ItemData data;

  public Item(ItemData data)
  {
    this.data = data;
  }
}

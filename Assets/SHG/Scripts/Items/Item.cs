using System;
using UnityEngine;
namespace SHG
{
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
}

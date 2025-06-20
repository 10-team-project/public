using System;
using UnityEngine;

namespace SHG
{
  public class ItemHolder : MonoBehaviour
  {
    public ItemData itemData;
    public Action<ItemData> OnDestroyed;

    void OnDestroy()
    {
      if (this.itemData != null && this.OnDestroyed != null) {
        this.OnDestroyed.Invoke(this.itemData);
      }
    }
  }
}

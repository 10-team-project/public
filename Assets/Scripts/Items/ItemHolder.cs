using System;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
  public ItemData ItemData;
  public Action<ItemData> OnDestroyed;

  void OnDestroy()
  {
    if (this.ItemData != null && this.OnDestroyed != null) {
      this.OnDestroyed.Invoke(this.ItemData);
    }
  }
}

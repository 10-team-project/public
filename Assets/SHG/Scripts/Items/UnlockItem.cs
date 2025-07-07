using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHG
{
  public class UnlockItem : Item, IUsable
  {
    public UnlockItem(DropChangeItemData data) : base(data)
    {

    }

    public Action<IUsable> OnUsed { get; set; }

  }
}

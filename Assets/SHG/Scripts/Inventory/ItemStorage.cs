using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using System;

namespace SHG
{
  public class ItemStorage : SingletonBehaviour<ItemStorage>, IObservableObject<ItemStorage>
  {
    public const int MAX_SLOT_COUNT = 20;
    public Action<ItemStorage> WillChange { get; set; }
    public Action<ItemStorage> OnChanged { get; set; }


  }
}

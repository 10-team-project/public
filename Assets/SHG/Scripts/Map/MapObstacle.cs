using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHG
{
  public interface IMapObject
  {
    public bool IsInteractable(EquipmentItemData item);
    public IEnumerator Interact(EquipmentItem item, Action OnEnded);
  }

  [RequireComponent(typeof(Collider))]
  public class MapObstacle : MonoBehaviour, IMapObject
  {
    public MapObjectData ObstacleData;

    public bool IsInteractable(EquipmentItemData item)
    {
      return (item.Purpose == EquipmentItemPurpose.Destruct);
    }

    public IEnumerator Interact(EquipmentItem item, Action OnEnded = null)
    {
      Debug.Log($"interact to {this.ObstacleData.Name} with {item.Data.Name}");
      Destroy(this.gameObject);
      yield return (null);
      OnEnded?.Invoke();
    }
  }
}

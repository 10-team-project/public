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
    DissolveController dissolveController;

    void Awake()
    {
      this.dissolveController = this.GetComponent<DissolveController>();
    }

    void Start()
    {
      if (this.dissolveController != null) {
        this.dissolveController.AppearImmediately();
      }
    }

    public bool IsInteractable(EquipmentItemData item)
    {
      return (
        item.Purpose == EquipmentItemPurpose.Destruct &&
        Array.IndexOf(this.ObstacleData.RequiredItems, item) != -1
        );
    }

    public IEnumerator Interact(EquipmentItem item, Action OnEnded = null)
    {
      Debug.Log($"interact to {this.ObstacleData.Name} with {item.Data.Name}");

      if (this.dissolveController != null) {
        yield return (this.dissolveController.StartDisappear());
      }
      Destroy(this.gameObject);
      yield return (null);
      OnEnded?.Invoke();
    }
  }
}

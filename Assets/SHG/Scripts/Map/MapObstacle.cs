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
      var player = GameObject.FindWithTag("Player");
      if (this.dissolveController != null) {
        if (player != null) {
          CameraController.FocusDirection focusDirection =
            player.transform.position.x < this.transform.position.x ? 
            CameraController.FocusDirection.Left:
            CameraController.FocusDirection.Right;
          App.Instance.CameraController.AddFocus(
            this.transform,
            focusDirection,
            onEnded: (cam) => {});
        }
        yield return (this.dissolveController.StartDisappear());
      }
      OnEnded?.Invoke();
      if (player != null) {
        App.Instance.CameraController.OnCommandEnd();
        App.Instance.CameraController.AddReset();
      }
      yield return (null);
      Destroy(this.gameObject);
    }
  }
}

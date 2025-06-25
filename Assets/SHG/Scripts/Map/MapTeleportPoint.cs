using System;
using UnityEngine;

namespace SHG
{
  [RequireComponent(typeof(Collider))] [Serializable]
  public class MapTeleportPoint : MonoBehaviour, IInteractable
  {
    public Action<MapTeleportPoint, Transform> OnTrigger;
    public Vector3 TeleportPosition => this.teleportPosition.position;
    [SerializeField]
    Transform teleportPosition;

    void Awake()
    {
      if (this.teleportPosition == null) {
        this.teleportPosition = this.transform;
      }
    }

    Transform GetPlayer()
    {
      return (GameObject.FindWithTag("Player").transform);
    }

    public void Interact()
    {
      Transform player = this.GetPlayer();
      this.OnTrigger?.Invoke(this, player); 
    }
  }
}

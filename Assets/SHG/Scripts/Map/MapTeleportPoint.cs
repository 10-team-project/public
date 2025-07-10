using System;
using LTH;
using UnityEngine;

namespace SHG
{
  [RequireComponent(typeof(Collider))]
  [Serializable]
  public class MapTeleportPoint : MonoBehaviour, IInteractable
  {
    public Action<MapTeleportPoint, Transform> OnTrigger;
    public Vector3 TeleportPosition => this.teleportPosition.position;
    [SerializeField, InspectorName("Optional teleport position")]
    Transform teleportPosition;

    void Awake()
    {
      if (this.teleportPosition == null)
      {
        this.teleportPosition = this.transform;
      }
    }

    Transform GetPlayer()
    {
      return (GameObject.FindWithTag("Player").transform);
    }

    public void Interact()
    {
      var popup = PopupManager.Instance.ShowPopup<ConfirmPopup>();
      if (popup == null) return;

      popup.Show("Teleport?","Yes", 
        () =>
        {
        App.Instance.AudioManager.PlaySFX(App.Instance.AudioManager.TeleportSound);
        Transform player = this.GetPlayer();
        this.OnTrigger?.Invoke(this, player);
        },

        "No",
        () => Debug.Log("텔레포트 취소됨")
        );
    }
  }
}

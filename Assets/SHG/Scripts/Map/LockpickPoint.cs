using System;
using System.Collections;
using UnityEngine;

namespace SHG
{
    public class LockpickPoint : DoorLocker, IMapObject
    {
      [SerializeField]
      ItemData lockpickItem;
      [SerializeField]
      float duration;
      PlayerItemController player;

      public IEnumerator Interact(EquipmentItem item, Action OnEnded)
      {
        yield return (new WaitForSeconds(this.duration));
        this.OnUnlock?.Invoke();
        OnEnded?.Invoke();
      }

      public bool IsInteractable(EquipmentItemData item)
      {
        if (this.player == null) {
          this.player = GameObject.FindWithTag("Player")?.GetComponent<PlayerItemController>();
        }
        if (this.player == null) {
          return (false);
        }
        return (true);
      }
    }
}

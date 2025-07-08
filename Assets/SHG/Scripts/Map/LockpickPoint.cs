using System;
using System.Collections;
using UnityEngine;

namespace SHG
{
    public class LockpickPoint : DoorLocker, IMapObject
    {
      const string LOCKPICK_ID = "8b68501d-c1b4-4a92-aaeb-923f0a98976c";
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
        return (item.Id == LOCKPICK_ID);
      }
    }
}

using System.Collections;
using UnityEngine;

namespace SHG
{
  [RequireComponent(typeof(Collider))]
  public class BreakBookcaseTrigger : MonoBehaviour, IMapObject
  {
    [SerializeField]
    BreakBookcaseController[] bookcases;
    [SerializeField]
    Transform focusTarget;
    [SerializeField]
    CameraController.FocusDirection focusDirection;
    Collider trigger;
    PlayerItemController player;

    void Awake()
    {
      this.trigger = this.GetComponent<Collider>();
    }

    public bool IsInteractable(EquipmentItemData item)
    {
      if (this.player == null) {
        this.player = GameObject.FindWithTag("Player")?.GetComponent<PlayerItemController>();
      }
      return (this.player != null);
    }

    public IEnumerator Interact(EquipmentItem item, System.Action OnEnded)
    {
      App.Instance.CameraController.AddFocus(
        this.focusTarget != null ? this.focusTarget: this.transform,
        this.focusDirection,
        (camera) => {});
      this.player.OnHit = this.OnHit;
      this.player.Pipe.SetActive(true);
      while (!this.bookcases[0].IsFinshed()) {
        this.player.TriggerAnimation("OneHandAttack"); 
        yield return (this.player.WaitForHitDelay);
      }
      this.trigger.enabled = false;
      OnEnded?.Invoke();
    }

    void Break()
    {
      foreach (var bookcase in this.bookcases) {
        if (!bookcase.IsFinshed()) {
          bookcase.Break();
        }
      }
    }

    void OnHit(PlayerItemController player)
    { 
      this.Break();
      if (this.bookcases[0].IsFinshed()) {
        App.Instance.CameraController.OnCommandEnd(); 
        App.Instance.CameraController.AddReset();
        player.OnHitFinish = this.OnFinshBreak;
      }
    }

    void OnFinshBreak(PlayerItemController player)
    {
      player.Pipe.SetActive(false);
      this.trigger.enabled = false;
    }
  }
}

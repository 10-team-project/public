using System;
using System.Collections;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class PadlockController : DoorLocker, IMapObject
  {
    [SerializeField] [Required]
    Rigidbody rb;
    [SerializeField] [Required]
    GameObject lockedUpperPart;
    [SerializeField] [Required]
    GameObject unlockedUpperPart;
    [SerializeField] [Required]
    Transform bodyPart; 
    [SerializeField] [Required]
    Transform bodyHinge;
    [SerializeReference]
    Transform focusPoint;
    [SerializeField]
    Vector3 forceDirection;
    [SerializeField] [Range (1f, 10f)]
    float impactPower;
    [SerializeField] [Range (0.1f, 2f)]
    float lockSpeed;
    [SerializeField] [Range(1f, 5f)]
    int numberOfHitsForUnlock;
    [SerializeField]
    CameraController.FocusDirection focusDirection;
    Coroutine lockRoutine;
    PlayerItemController player;

    void Awake()
    {
      this.IsLocked = true;

    }

    [Button ("Hit")]
    public void HitTest()
    {
      this.Hit(this.forceDirection, this.impactPower);
    }

    public void Hit(Vector3 forceDirection, float impactPower)
    {
      this.rb.AddForceAtPosition(
        forceDirection * impactPower,
        this.bodyPart.position,
        ForceMode.Impulse
        );
    }

    [Button ("Lock")]
    public void Lock()
    {
      this.rb.velocity = Vector3.zero;
      this.lockedUpperPart.SetActive(true);
      this.unlockedUpperPart.SetActive(false);
      this.IsLocked = true;
    }

    [Button ("Unlock")]
    public void UnLock()
    {
      this.lockedUpperPart.SetActive(false);
      this.unlockedUpperPart.SetActive(true);
      this.IsLocked = false;
      this.OnUnlock?.Invoke();
    }

    [Button ("Toggle lock")]
    public void ToggleLock()
    {
      if (this.lockRoutine != null) {
        this.StopCoroutine(this.lockRoutine);
        this.lockRoutine = null;
      }
      float destAngle = this.IsLocked ? 180f: -180f;
      if (this.IsLocked) {
        this.UnLock();
        this.lockRoutine = 
          this.StartCoroutine(this.CreateLockRoutine(destAngle));
      }
      else {
        this.lockRoutine = 
          this.StartCoroutine(this.CreateLockRoutine(destAngle, this.Lock));
      }
    }

    IEnumerator CreateLockRoutine(float destAngle, Action OnEnded = null)
    {
      float currentAngle = 0f;
      float deltaAngle = destAngle * Time.deltaTime * this.lockSpeed;
      if (currentAngle < destAngle) {
        while (currentAngle < destAngle) {
          currentAngle += deltaAngle;
          this.RotateBody(deltaAngle);
          yield return (null);
        }
      }
      else {
        while (currentAngle > destAngle) {
          currentAngle += deltaAngle;
          this.RotateBody(deltaAngle);
          yield return (null);
        }
      }
      OnEnded?.Invoke();
    }

    [Button ("Rotate body")]
    public void RotateBody(float angle)
    {
      this.bodyPart.RotateAround(
        this.bodyHinge.position,
        this.transform.up,
        angle
        );
    }

    public bool IsInteractable(EquipmentItemData item)
    {
      if (this.player == null) {
        this.player = GameObject.FindWithTag("Player")?.GetComponent<PlayerItemController>();
      }
      if (this.player == null) {
        return (false);
      }
      return true;
    }

    public IEnumerator Interact(EquipmentItem item, Action OnEnded)
    {
      //TODO: check item
      #if UNITY_EDITOR
      if (this.numberOfHitsForUnlock < 1) {
        Debug.LogError($"invalid numberOfHitsForUnlock: {this.numberOfHitsForUnlock}");
      }
      #endif
      App.Instance.CameraController.AddFocus(
        this.focusPoint != null ? this.focusPoint.transform:
        this.transform,
        this.focusDirection,
        (camera) => {});
      int count = 1;
      player.OnHit = (player) => {
        this.OnHit(player, count);
        if (count >= this.numberOfHitsForUnlock) {
          OnEnded?.Invoke();
        }
        count += 1;
      };
      for (int i = 0; i < this.numberOfHitsForUnlock; i++) {
        this.player.TriggerAnimation("OneHandAttack");
        yield return (this.player.WaitForHitDelay);
      }
      yield return null;
    }

    void OnHit(PlayerItemController player, int count)
    { 
      this.Hit(this.forceDirection, this.impactPower);
      if (count >= this.numberOfHitsForUnlock && this.IsLocked) {
        this.ToggleLock();
        player.OnHit = null;
        App.Instance.CameraController.OnCommandEnd();
        App.Instance.CameraController.AddReset();
      }
    }
  }
}

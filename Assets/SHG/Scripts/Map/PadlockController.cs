using System;
using System.Collections;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [RequireComponent (typeof(Rigidbody), typeof(CharacterJoint))]
  public class PadlockController : MonoBehaviour
  {
    public bool IsLocked { get; private set; }
    [SerializeField] [Required]
    GameObject lockedUpperPart;
    [SerializeField] [Required]
    GameObject unlockedUpperPart;
    [SerializeField] [Required]
    Transform bodyPart; 
    [SerializeField] [Required]
    Transform bodyHinge;
    [SerializeField]
    Vector3 forceDirection;
    [SerializeField] [Range (1f, 10f)]
    float impactPower;
    [SerializeField] [Range (0.1f, 2f)]
    float lockSpeed;
    Rigidbody rb;
    Coroutine lockRoutine;

    void Awake()
    {
      this.rb = this.GetComponent<Rigidbody>();
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
  }
}

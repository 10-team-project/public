using System.Collections;
using System.Collections.Generic;
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
    Rigidbody rb;

    void Awake()
    {
      this.rb = this.GetComponent<Rigidbody>();
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
      this.lockedUpperPart.SetActive(true);
      this.unlockedUpperPart.SetActive(false);
      this.RotateBody(180f);
      this.IsLocked = true;
    }

    [Button ("Unlock")]
    public void UnLock()
    {
      this.lockedUpperPart.SetActive(false);
      this.unlockedUpperPart.SetActive(true);
      this.RotateBody(-180f);
      this.IsLocked = false;
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

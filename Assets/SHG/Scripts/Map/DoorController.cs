using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class DoorController : MonoBehaviour, IInteractable
  {
    public bool IsClosed { get; private set; }
    [SerializeField] [Required]
    Transform doorHinge;
    [SerializeField] 
    float openedAngle;
    [SerializeField] 
    float closedAngle;
    [SerializeField] [Range (1f, 30f)]
    float rotateSpeed;
    [SerializeField]
    bool isClosed;
    Coroutine rotateRoutine;
    Quaternion opendedRotation;
    Quaternion closedRotation;
    Quaternion destRotation;

    void Awake()
    {
      this.opendedRotation = Quaternion.Euler(0, this.openedAngle, 0);
      this.closedRotation = Quaternion.Euler(0, this.closedAngle, 0);
      this.IsClosed = this.isClosed;
    }

    [Button ("Open")]
    void Open()
    {
      if (this.rotateRoutine != null) {
        this.StopCoroutine(this.rotateRoutine);
        this.rotateRoutine = null;
        this.doorHinge.rotation = this.destRotation;
      }
      this.destRotation = this.opendedRotation;
      this.rotateRoutine = this.StartCoroutine(this.CreateRotateRoutine());
      this.IsClosed = false;
    }

    [Button ("Close")]
    void Close()
    {
      if (this.rotateRoutine != null) {
        this.StopCoroutine(this.rotateRoutine);
        this.rotateRoutine = null;
        this.doorHinge.rotation = this.destRotation;
      }
      this.destRotation = this.closedRotation;
      this.rotateRoutine = this.StartCoroutine(this.CreateRotateRoutine());
      this.IsClosed = true; 
    }

    IEnumerator CreateRotateRoutine()
    {
      while (Quaternion.Angle(
          this.doorHinge.rotation,
          this.destRotation 
          ) > float.Epsilon) {
        this.doorHinge.rotation = Quaternion.Lerp(
          this.doorHinge.rotation,
          this.destRotation,
          this.rotateSpeed * Time.deltaTime
          );
        yield return (null);
      } 
      this.doorHinge.rotation = this.destRotation;
    }

    public void Interact()
    {
      if (this.IsClosed) {
        this.Open();
      }
      else {
        this.Close();
      }
    }
  }
}

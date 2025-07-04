using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class DoorController : MonoBehaviour
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
    Coroutine rotateRoutine;
    Quaternion opendedRotation;
    Quaternion closedRotation;
    Quaternion destRotation;

    void Awake()
    {
      this.opendedRotation = Quaternion.Euler(0, this.openedAngle, 0);
      this.closedRotation = Quaternion.Euler(0, this.closedAngle, 0);
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
  }

}

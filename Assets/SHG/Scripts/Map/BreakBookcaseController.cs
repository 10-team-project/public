using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class BreakBookcaseController : MonoBehaviour, IMapObject
  {
    [SerializeField] [Required]
    Transform brokenPiecesContainer;
    [SerializeField] [Required]
    Transform doorsContainer;
   
    [SerializeField] 
    int numberOfPieceBreakedAtOnce;
    [SerializeField] [Range (-1f, 0f)]
    float minXAngle;
    [SerializeField] [Range (0f, 1f)]
    float maxXAngle;
    [SerializeField] [Range (-1f, 0f)]
    float minZAngle;
    [SerializeField] [Range (0f, 1f)]
    float maxZAngle;
    [SerializeField] [Range (0, 5f)]
    float minBreakForce;
    [SerializeField] [Range (5f, 10f)]
    float maxBreakForce;
    [SerializeField] [ReadOnly]
    Transform[] brokenPieces;
    [SerializeField] [ReadOnly]
    Transform[] doors;
    HashSet<int> fellAwayedParts;
    PlayerItemController player;

    void Awake()
    {
      this.numberOfPieceBreakedAtOnce = Mathf.Max(this.numberOfPieceBreakedAtOnce, 1);
      this.brokenPieces = new Transform[this.brokenPiecesContainer.childCount];
      for (int i = 0; i < this.brokenPiecesContainer.childCount; i++) {
        this.brokenPieces[i] = this.brokenPiecesContainer.GetChild(i);
      }
      this.doors = new Transform[this.doorsContainer.childCount];
      for (int i = 0; i < this.doorsContainer.childCount; i++) {
        this.doors[i] = this.doorsContainer.GetChild(i);
      }
      this.fellAwayedParts = new ();
    }

    Vector3 GetDirection()
    {
      return (new Vector3(
          Random.Range(this.minXAngle, this.maxXAngle),
          Random.Range(0f, 1f),
          Random.Range(this.minZAngle, this.maxZAngle)
          ).normalized);
    }

    [Button ("Break")]
    void Break()
    {
      #if UNITY_EDITOR
      if (this.IsFinshed()) {
        Debug.Log("breaking finished");
        return ;
      }
      #endif
      for (int i = 0; i < this.numberOfPieceBreakedAtOnce && 
        !this.IsFinshed(); i++) {
        Transform part = this.GetNextPart();
        var rb = part.gameObject.AddComponent<Rigidbody>();
        Vector3 dir = this.GetDirection();
        float magnitude = Random.Range(this.minBreakForce, this.maxBreakForce);
        rb.AddForce(
          dir * magnitude,
          ForceMode.Impulse
          );
        rb.AddRelativeTorque(
          Vector3.Cross(dir, Vector3.up),
          ForceMode.Impulse
          );
      }
    }

    bool IsFinshed()
    {
      return (this.fellAwayedParts.Count >= this.brokenPieces.Length + this.doors.Length);
    }

    Transform GetNextPart()
    {
      int nextIndex = this.fellAwayedParts.Count;
      this.fellAwayedParts.Add(nextIndex);
      if (nextIndex < this.brokenPieces.Length) {
        return (this.brokenPieces[nextIndex]);
      } 
      else {
        nextIndex -= this.brokenPieces.Length;
        return (this.doors[nextIndex]);
      }
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
        this.transform,
        CameraController.FocusDirection.Foward,
        (camera) => {},
        0.5f
        );
      this.player.OnHit = this.OnHit;
      while (!this.IsFinshed()) {
        this.player.TriggerAnimation("Hit"); 
        yield return (this.player.WaitForHitDelay);
      }
    }

    void OnHit(PlayerItemController player)
    { 
      this.Break();
      if (this.IsFinshed()) {
        App.Instance.CameraController.OnCommandEnd(); 
        App.Instance.CameraController.AddReset();
      }
    }
  }
}

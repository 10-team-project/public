using System;
using System.Collections;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [RequireComponent(typeof(Collider))]
  public class MapConstructPoint : MonoBehaviour, IMapObject
  {
    [SerializeField]
    public MapConstructPointData PointData;
    public bool IsCosntructed { get; private set; }
    [SerializeField]
    GameObject placeHolder;
    [SerializeField, ReadOnly]
    public GameObject Construction;
    [SerializeField]
    Transform spawnPoint;
    DissolveController dissolveController;
    Collider blockingCollider;

    [Button ("Construct")]
    public void ConstructTest()
    {
      this.StartCoroutine(this.Interact(null));
    }

    public void Construct()
    {
      if (this.PointData == null) {
        #if UNITY_EDITOR
        Debug.LogError("No data to construct");
        #endif
        return ;
      }
      if (this.Construction != null) {
        #if UNITY_EDITOR
        Debug.LogError("Cosntruction is not null");
        #endif
        return;
      }
      this.Construction = Instantiate(this.PointData.Prefab);
      this.Construction.transform.SetParent(this.transform);
      if (this.spawnPoint != null) {
        this.Construction.transform.position = this.spawnPoint.position;
        this.Construction.transform.rotation = this.spawnPoint.rotation;
      }
      else {
        this.Construction.transform.position = this.transform.position;
      }
      if (this.placeHolder != null) {
        this.Construction.transform.localScale = this.placeHolder.transform.localScale;
      }
      this.dissolveController = this.Construction.GetComponent<DissolveController>();
    }

    public IEnumerator Interact(EquipmentItem item, Action OnEnded = null)
    {
      this.Construct();
      if (this.placeHolder != null) {
        this.placeHolder.SetActive(false);
      }
      if (this.dissolveController != null) {
        this.dissolveController.DisappearImmediately();
        App.Instance.CameraController.AddFocus(
          this.transform,
          CameraController.FocusDirection.Foward,
          onEnded: (camera) => {}
          );
        yield return (this.dissolveController.StartAppear());
      }
      App.Instance.CameraController.OnCommandEnd();
      App.Instance.CameraController.AddReset();
      if (this.blockingCollider != null) {
        this.blockingCollider.enabled = false;
      }
      OnEnded?.Invoke();
    }

    public bool IsInteractable(EquipmentItemData item)
    {
      if (this.PointData == null) {
        #if UNITY_EDITOR
        Debug.LogError("IsInteractable: Construction data is none");
        #endif
        return (false);
      }
      return (
        item.Purpose == EquipmentItemPurpose.Construct &&
        Array.IndexOf(this.PointData.RequiredItems, item) != -1);
    }

    void Awake()
    {
      this.blockingCollider = this.GetComponent<Collider>();
    }
  }
}

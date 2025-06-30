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
    DissolveController dissolveController;

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
      this.Construction.transform.localPosition = Vector3.zero;
      this.dissolveController = this.Construction.GetComponent<DissolveController>();
    }

    public IEnumerator Interact(EquipmentItem item, Action OnEnded = null)
    {
      if (this.placeHolder != null) {
        this.placeHolder.SetActive(false);
      }
      this.Construct();
      if (this.dissolveController != null) {
        this.dissolveController.DisappearImmediately();
        yield return (this.dissolveController.StartAppear());
      }
      yield return (null);
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
  }
}

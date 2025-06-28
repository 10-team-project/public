using System;
using System.Collections.Generic;
using UnityEngine;

namespace SHG
{
  public class PlayerItemController : MonoBehaviour
  {
    [SerializeField] [Range(0.5f, 5f)]
    float mapObjectIntractDist;
    [SerializeField] [Range(0.5f, 2f)]
    float mapObjectIntractRadius;
    LayerMask mapObjectLayer;
    [SerializeField] 
    Vector3 footOffset;
    Coroutine itemAction;

    void Awake()
    {
      this.mapObjectLayer = (1 << LayerMask.NameToLayer("Blocker"));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      Nullable<int> quickSlot = this.GetPressedQuickslot();
      if (this.itemAction == null && quickSlot != null &&
        App.Instance.UIController.TryGetQuickSlotItem(quickSlot.Value, out EquipmentItemData itemData)) {
        if (this.TryFindMapObject(itemData, out IMapObject mapObject)) {
          this.TryInteractWith(itemData, mapObject);
        }
      }
    }

    void TryInteractWith(EquipmentItemData itemData, IMapObject mapObject)
    {
      if (mapObject.IsInteractable(itemData)) {
        EquipmentItem item = App.Instance.Inventory.GetItemFromQuickSlot(itemData);
        //TODO: lock input
        this.itemAction = this.StartCoroutine(
          mapObject.Interact(item, this.OnItemActionFinished));
      }
    }

    void OnItemActionFinished()
    {
      //TODO: release input
      this.itemAction = null;
    }

    bool TryFindMapObject(EquipmentItemData item, out IMapObject mapObject)
    {
      bool isHit = Physics.SphereCast(
        origin: this.transform.position + this.footOffset,
        radius: this.mapObjectIntractRadius,
        direction: this.transform.forward,
        hitInfo: out RaycastHit hitInfo,
        maxDistance: this.mapObjectIntractDist,
        layerMask: this.mapObjectLayer);
      #if UNITY_EDITOR
      Debug.DrawRay(
        this.transform.position + this.footOffset,
        this.transform.forward * this.mapObjectIntractDist,
        Color.red,
        0.3f
        );
      #endif
      mapObject = null;
      if (isHit) {
        mapObject = (hitInfo.collider.GetComponent<IMapObject>());
      }
      return (mapObject != null);
    }

    Nullable<int> GetPressedQuickslot()
    {
      for (int i = 0; i < Settings.InputSettings.QuickSlotKeys.Length; i++) {
        if (Input.GetKeyDown(Settings.InputSettings.QuickSlotKeys[i])) {
          return (i);
        }
      }
      return (null);
    }
  }
}

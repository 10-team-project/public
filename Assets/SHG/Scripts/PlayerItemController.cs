using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using EditorAttributes;

namespace SHG
{
  [RequireComponent(typeof(Animator))]
  public class PlayerItemController : MonoBehaviour
  {
    public Action<PlayerItemController> OnHit;
    public Action OnMidLootItem;
    public Action OnEndLootItem;
    public WaitForSeconds WaitForHitDelay;
    [SerializeField] [Required]
    Transform headAimTarget;
    [SerializeField] [Required]
    MultiAimConstraint headAim;
    [SerializeField] [Required]
    MultiAimConstraint bodyAim;
    [SerializeField] [Required]
    Transform leftHandTarget;
    [SerializeField] [Required]
    Transform rightHandTarget;
    [SerializeField] [Range(1f, 3f)]
    float lookAtSpeed;
    [SerializeField] [Range(0.5f, 5f)]
    float mapObjectIntractDist;
    [SerializeField] [Range(0.5f, 2f)]
    float mapObjectIntractRadius;
    LayerMask mapObjectLayer;
    [SerializeField] 
    Vector3 footOffset;
    [SerializeField] [Range (0f, 5f)]
    float hitDelay;
    Coroutine itemAction;
    Coroutine lookAction;
    Coroutine lootAction;
    Animator animator;
    ItemObject itemToLoot;

    public void TriggerAnimation(string name)
    {
      this.animator.SetTrigger(name);
    }

    public void LootItem(ItemObject item)
    {
      this.itemToLoot = item;
      this.leftHandTarget.position = item.transform.position;
      this.TriggerAnimation("LootItem");
    }

    void Awake()
    {
      this.animator = this.GetComponent<Animator>();
      this.mapObjectLayer = (1 << LayerMask.NameToLayer("ItemInteractObject"));
      this.WaitForHitDelay = new WaitForSeconds(this.hitDelay);
    }

    void LookTarget(Transform target)
    {
      if (this.lookAction != null) {
        this.ClearLookAction();
      }
      this.headAimTarget.position = target.position;
      this.lookAction = this.StartCoroutine(this.StartLookAt(target));
    }

    [Button ("Look at")]
    void LookAt(Vector3 position)
    {
      if (this.lookAction != null) {
        this.ClearLookAction();
      }
      this.headAimTarget.position = position;
      this.lookAction = this.StartCoroutine(this.StartLookAt());
    }

    [Button ("Look foward")]
    void LookForward()
    {
      if (this.lookAction != null) {
        this.ClearLookAction();
      }
      this.lookAction = this.StartCoroutine(this.StartLookFoward());
    }

    void ClearLookAction()
    {
      this.StopCoroutine(this.lookAction);
      this.lookAction = null;
    }

    IEnumerator StartLookAt(Transform target = null)
    {
      if (target != null) {
        while (this.headAim.weight <= 1f) {
          this.headAim.weight = Mathf.Lerp(
            this.headAim.weight,
            1f,
            this.lookAtSpeed * Time.deltaTime
            );
          this.bodyAim.weight = Mathf.Lerp(
            this.headAim.weight,
            0.5f,
            this.lookAtSpeed * Time.deltaTime
            );

          this.headAimTarget.position = target.position;
          yield return (null);
        }
      }
      else {
        while (this.headAim.weight <= 1f) {
          this.headAim.weight =  Mathf.Lerp(
            this.headAim.weight,
            1f,
            this.lookAtSpeed * Time.deltaTime
            );
          this.bodyAim.weight = Mathf.Lerp(
            this.headAim.weight,
            0.5f,
            this.lookAtSpeed * Time.deltaTime
            );
          yield return (null);
        }
      }
      this.headAim.weight = 1f;
      this.bodyAim.weight = 0.5f;
    }

    IEnumerator StartLookFoward()
    {
      float weight = 0f;
      while (this.headAim.weight >= 0f) {
        weight = Mathf.Lerp(
          this.headAim.weight,
          0f,
          this.lookAtSpeed * Time.deltaTime
          );
        this.headAim.weight = weight;
        this.bodyAim.weight = weight;
        yield return (null);
      }
      this.headAim.weight = 0f;
      this.bodyAim.weight = 0f;
    }

    void OnHitTrigger()
    {
      this.OnHit?.Invoke(this);
    }

    void OnMidPointLoot()
    {
      if (this.itemToLoot != null && this.leftHandTarget != null) {
        this.itemToLoot.transform.SetParent(this.leftHandTarget.transform);
        this.itemToLoot.transform.position = this.leftHandTarget.position;
      }
    }

    void OnEndPointLoot()
    {
      if (this.itemToLoot != null) {
        this.itemToLoot.Pickup();
        this.itemToLoot = null;
      }
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
        else {
          Debug.Log("map object not found");
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
      else {
        Debug.Log($"not interactable with {itemData.Name} + {mapObject}");
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

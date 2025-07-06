using System;
using System.Collections;
using System.Collections.Generic;
using SHG;
using UnityEngine;


namespace LTH
{
    [RequireComponent(typeof(Collider))]
    public class LadderPlacePoint : MonoBehaviour, IMapObject, IInteractable
    {
        [SerializeField] public LadderPlacePointData PointData;
        [SerializeField] GameObject placeHolder;
        [SerializeField] Transform spawnPoint;
        [SerializeField] Collider blockingCollider;

        private GameObject construction;
        private DissolveController dissolveController;

        public bool IsConstructed => construction != null;

        public void Construct()
        {
            if (PointData == null)
            {
#if UNITY_EDITOR
                Debug.LogError("[LadderPlacePoint] Missing PointData");
#endif
                return;
            }

            if (construction != null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("[LadderPlacePoint] Already Constructed");
#endif
                return;
            }

            construction = Instantiate(PointData.Prefab);
            construction.transform.SetParent(transform);

            if (this.placeHolder != null) {
              construction.transform.rotation = this.placeHolder.transform.rotation;
            }

            if (spawnPoint != null)
            {
                construction.transform.position = spawnPoint.position;
                construction.transform.rotation = spawnPoint.rotation;
                construction.transform.Rotate(0f, -90f, 0f);
            }
            else
            {
                construction.transform.position = transform.position;
                construction.transform.Rotate(0f, -90f, 0f);
            }
            dissolveController = construction.GetComponent<DissolveController>();
        }

        public void Interact() // IInteractable Test용
        {
#if UNITY_EDITOR
            EquipmentItemData dummyItemData = ScriptableObject.CreateInstance<EquipmentItemData>();
            typeof(EquipmentItemData)
                .GetField("usePurpose", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(dummyItemData, EquipmentItemPurpose.Construct);

            EquipmentItem dummyItem = new EquipmentItem(dummyItemData);
            StartCoroutine(Interact(dummyItem, () => Debug.Log("F키 눌러 상호작용 확인")));
#endif
        }


        public IEnumerator Interact(EquipmentItem item, Action OnEnded = null)
        {
            if (placeHolder != null) placeHolder.SetActive(false);

            App.Instance.CameraController.AddFocus(
              transform,
              CameraController.FocusDirection.Foward);
            yield return (new WaitForSeconds(0.5f));
            Construct();

            if (dissolveController != null)
            {
                dissolveController.DisappearImmediately();

                yield return dissolveController.StartAppear();
            }
            else if (!construction.activeSelf){
              construction.SetActive(true);
            }

            if (blockingCollider != null) blockingCollider.enabled = false;

            yield return null;
            OnEnded?.Invoke();
        }

        public bool IsInteractable(EquipmentItemData item)
        {
          return (true);
            if (PointData == null) return false;

            return item.Purpose == EquipmentItemPurpose.Construct &&
                   Array.IndexOf(PointData.RequiredItems, item) != -1;
        }

        void Awake()
        {
            if (blockingCollider == null)
                blockingCollider = GetComponent<Collider>();
        }

#if UNITY_EDITOR
        [ContextMenu("TEST: Place Ladder")]
        public void TestPlaceLadder()
        {
            EquipmentItemData dummyItemData = ScriptableObject.CreateInstance<EquipmentItemData>();
            dummyItemData.name = "Dummy Ladder";
            
            typeof(EquipmentItemData)
       .GetField("usePurpose", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
       .SetValue(dummyItemData, EquipmentItemPurpose.Construct);

            EquipmentItem dummyItem = new EquipmentItem(dummyItemData);

            StartCoroutine(Interact(dummyItem, () => Debug.Log("Ladder placed in test")));
        }
#endif
    }
}

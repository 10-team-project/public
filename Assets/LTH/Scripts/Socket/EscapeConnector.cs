using System.Collections.Generic;
using SHG;
using UnityEngine;

// 연결방법 : App.cs에서 Inventory와 ItemTracker를 생성한 후 EscapeConnector를 생성하여 연결합니다.

// App.cs => Awake()
// this.Inventory = new Inventory();
// this.ItemTracker = new ItemTracker(this.Inventory);
// new EscapeConnector(this.ItemTracker, this.Inventory); => 추가하면 됨

namespace LTH
{
    public class EscapeConnector : MonoBehaviour
    {
        private readonly ItemTracker itemTracker;
        private readonly Inventory inventory;

        public bool IsEscapeReady { get; private set; }
        public EscapeConnector(ItemTracker tracker, Inventory inventory)
        {
            this.itemTracker = tracker;
            this.inventory = inventory;

            // 아이템을 새로 획득할 때마다 조건 검사
            this.itemTracker.ConsumeNewObtainedItems(CheckEscapeCondition);
        }

        /// <summary>
        /// 탈출 조건을 평가합니다.
        /// </summary>
        private void CheckEscapeCondition(List<ItemData> _)
        {
            List<ItemData> currentItems = GetAllInventoryItems();
            IsEscapeReady = EscapeManager.Instance.CheckInventoryForEscapeSuccess(currentItems);

            if (IsEscapeReady)
            {
                EscapeManager.Instance.EscapeSuccess();
                TriggerEscapeSuccess();
            }
            else
            {
                EscapeManager.Instance.EscapeFailure();
                TriggerEscapeFailure();
            }
        }

        /// <summary>
        /// 인벤토리 내 모든 아이템을 수량 포함해서 리스트로 반환
        /// </summary>
        private List<ItemData> GetAllInventoryItems()
        {
            var result = new List<ItemData>();

            foreach (var (item, count) in inventory.Items)
            {
                for (int i = 0; i < count; i++)
                {
                    result.Add(item);
                }
            }
            return result;
        }


        // 아이템이 충족하는 순간 연출 (탈출 시에 연출이 아님)
        private void TriggerEscapeSuccess()
        {
            Debug.Log("탈출 조건이 충족되었습니다.");
        }

        private void TriggerEscapeFailure()
        {
            Debug.Log("탈출 조건이 아직 충족되지 않았습니다.");
        }
    }
}
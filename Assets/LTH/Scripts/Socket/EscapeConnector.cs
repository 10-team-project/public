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

        public EscapeConnector(ItemTracker tracker, Inventory inventory)
        {
            this.itemTracker = tracker;
            this.inventory = inventory;

            this.itemTracker.ConsumeNewObtainedItems(CheckEscapeCondition);
        }

        private void CheckEscapeCondition(List<ItemData> _)
        {
            List<ItemData> inventoryItems = GetAllInventoryItems();

            bool success = EscapeManager.Instance.CheckInventoryForEscapeSuccess(inventoryItems);

            if (success)
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

        private List<ItemData> GetAllInventoryItems()
        {
            List<ItemData> inventoryItems = new();

            foreach (var pair in inventory.Items)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    inventoryItems.Add(pair.Key);
                }
            }
            return inventoryItems;
        }


        private void TriggerEscapeSuccess()
        {
            Debug.Log("게임 클리어! 탈출 성공");
            // TODO: 게임 클리어 이벤트(대사), 해피엔딩 UI 출력 등 탈출 후 처리 로직 작성
        }

        private void TriggerEscapeFailure()
        {
            Debug.Log("탈출 조건이 아직 충족되지 않았습니다.");
            // TODO: 게임 오버 이벤트(대사), 배드엔딩 UI 등 게임 오버 후 처리 로직 작성
        }
    }
}
using System.Collections.Generic;
using SHG;
using UnityEngine;

// ������ : App.cs���� Inventory�� ItemTracker�� ������ �� EscapeConnector�� �����Ͽ� �����մϴ�.

// App.cs => Awake()
// this.Inventory = new Inventory();
// this.ItemTracker = new ItemTracker(this.Inventory);
// new EscapeConnector(this.ItemTracker, this.Inventory); => �߰��ϸ� ��

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
            Debug.Log("���� Ŭ����! Ż�� ����");
            // TODO: ���� Ŭ���� �̺�Ʈ(���), ���ǿ��� UI ��� �� Ż�� �� ó�� ���� �ۼ�
        }

        private void TriggerEscapeFailure()
        {
            Debug.Log("Ż�� ������ ���� �������� �ʾҽ��ϴ�.");
            // TODO: ���� ���� �̺�Ʈ(���), ��忣�� UI �� ���� ���� �� ó�� ���� �ۼ�
        }
    }
}
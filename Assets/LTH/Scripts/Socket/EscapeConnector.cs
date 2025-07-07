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

        public bool IsEscapeReady { get; private set; }
        public EscapeConnector(ItemTracker tracker, Inventory inventory)
        {
            this.itemTracker = tracker;
            this.inventory = inventory;

            // �������� ���� ȹ���� ������ ���� �˻�
            this.itemTracker.ConsumeNewObtainedItems(CheckEscapeCondition);
        }

        /// <summary>
        /// Ż�� ������ ���մϴ�.
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
        /// �κ��丮 �� ��� �������� ���� �����ؼ� ����Ʈ�� ��ȯ
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


        // �������� �����ϴ� ���� ���� (Ż�� �ÿ� ������ �ƴ�)
        private void TriggerEscapeSuccess()
        {
            Debug.Log("Ż�� ������ �����Ǿ����ϴ�.");
        }

        private void TriggerEscapeFailure()
        {
            Debug.Log("Ż�� ������ ���� �������� �ʾҽ��ϴ�.");
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using SHG;
using UnityEngine;


namespace LTH
{
    public class ProbabilityItemSpawner : MonoBehaviour
    {
        [SerializeField] private List<ItemSpawnRule> spawnRules;
        [SerializeField] private DropTable dropTable;

        private void Start()
        {
            SpawnAll();
        }

        public void SpawnAll()
        {
            foreach (var rule in spawnRules)
            {
                var selectedGrade = ChooseGrade(rule.gradeEntries);
                if (selectedGrade == null || selectedGrade.items.Count == 0) continue;

                var item = selectedGrade.items[UnityEngine.Random.Range(0, selectedGrade.items.Count)];

                ItemObject instance = Item.CreateItemObjectFrom(item);
                instance.transform.position = rule.spawnPoint.position;
            }
        }

        private ItemProbabilityEntry ChooseGrade(List<ItemProbabilityEntry> grades)
        {
            float rand = UnityEngine.Random.value;
            float cumulative = 0f;

            List<ItemData> removedItems;
            List<ItemData> addedItems;

            if (dropTable != null)
            {
                removedItems = dropTable.GetRemovedItems();
                addedItems = dropTable.GetAddedItems();
            }
            else
            {
                removedItems = new List<ItemData>();
                addedItems = new List<ItemData>();
            }


            foreach (var entry in grades)
            {
                // DropTable ���� ���͸�
                bool allBlocked = entry.items.TrueForAll(item =>
                    removedItems.Contains(item) && !addedItems.Contains(item));
                if (allBlocked) continue;

                // 1ȸ�� ������ üũ
                if (entry.onlySpawnOnce &&
                    GameProgressManager.Instance.IsItemUsed(entry.itemIdentifier))
                    continue;

                // Ư�� ������ ��� �� ����
                if (entry.blockedIfUsedItem != null)
                {
                    foreach (var id in entry.blockedIfUsedItem)
                    {
                        if (GameProgressManager.Instance.IsItemUsed(id))
                            goto ContinueLoop;
                    }
                }

                // Ư�� ������ ȹ�� �� ����
                if (entry.blockedIfObtainedItem != null)
                {
                    foreach (var id in entry.blockedIfObtainedItem)
                    {
                        if (GameProgressManager.Instance.IsItemObtained(id))
                            goto ContinueLoop;
                    }
                }

                // Ư�� ������ ȹ�� �Ŀ��� ����
                if (entry.requiredObtainedItem != null)
                {
                    foreach (var id in entry.requiredObtainedItem)
                    {
                        if (!GameProgressManager.Instance.IsItemObtained(id))
                            goto ContinueLoop;
                    }
                }
                cumulative += entry.probability;
                if (rand <= cumulative)
                    return entry;

                ContinueLoop:;
            }
            return null;
        }
    }
}
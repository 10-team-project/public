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

        void Awake()
        {
          this.dropTable = App.Instance.DropTable;
          this.CopySettings(); 
        }

        void CopySettings()
        {
          if (this.spawnRules.Count < 1) {
            return ;
          }
          var sample = this.spawnRules[0];
          foreach (var point in this.transform) {
            if (point is Transform transform) {
              this.spawnRules.Add(
                new ItemSpawnRule {
                spawnPoint = transform,
                gradeEntries = sample.gradeEntries
                });
              transform.gameObject.SetActive(false);
            }
          }
        }

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

        private bool CheckSpawnConditions(List<SpawnCondition> conditions)
        {
            if (conditions == null || conditions.Count == 0) return true;

            foreach (var cond in conditions)
            {
                switch (cond.conditionType)
                {
                    case SpawnConditionType.Used:
                        if (GameProgressManager.Instance.IsItemUsed(cond.itemName))
                            return false;
                        break;

                    case SpawnConditionType.Obtained:
                        if (GameProgressManager.Instance.IsItemObtained(cond.itemName))
                            return false;
                        break;

                    case SpawnConditionType.RequireObtained:
                        if (!GameProgressManager.Instance.IsItemObtained(cond.itemName))
                            return false;
                        break;

                    case SpawnConditionType.SingleSpawn:
                        if (GameProgressManager.Instance.IsItemUsed(cond.itemName))
                            return false;
                        break;
                }
            }
            return true;
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
                if (IsBlockedByDropTable(entry, removedItems, addedItems))
                    continue;

                if (!CheckSpawnConditions(entry.spawnConditions))
                    continue;

                cumulative += entry.probability;
                if (rand <= cumulative)
                    return entry;
            }
            return null;
        }

        private bool IsBlockedByDropTable(ItemProbabilityEntry entry, List<ItemData> removed, List<ItemData> added)
        {
            // 모든 아이템이 제거됐고, 다시 추가도 안 됐다면 → 막힘
            return entry.items.TrueForAll(item => removed.Contains(item) && !added.Contains(item));
        }
    }
}

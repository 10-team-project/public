using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


namespace LTH
{
    public class ProbabilityItemSpawner : MonoBehaviour
    {
        [SerializeField] private List<ItemSpawnRule> spawnRules;

        private readonly HashSet<Transform> usedSpawnPoints = new();

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
            if (point is GameObject go) {
              this.spawnRules.Add(
                new ItemSpawnRule {
                spawnPoint = go.transform,
                gradeEntries = sample.gradeEntries
                });
              go.SetActive(false);
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

                //GameObject itemObject = Instantiate(item.Prefab);
                ItemObject instance = Item.CreateItemObjectFrom(item);
                instance.transform.position = rule.spawnPoint.position;

//                if (instance.TryGetComponent<ItemObject>(out var itemObject))
//                {
//                    itemObject.SetItem(Item.CreateItemFrom(item));
//                }
            }
        }

        private ItemProbabilityEntry ChooseGrade(List<ItemProbabilityEntry> grades)
        {
            float rand = UnityEngine.Random.value;
            float cumulative = 0f;

            foreach (var entry in grades)
            {
                cumulative += entry.probability;
                if (rand <= cumulative)
                {
                    return entry;
                }
            }
            return null;
        }
    }
}

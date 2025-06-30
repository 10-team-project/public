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

                GameObject instance = Instantiate(item.Prefab);
                instance.transform.position = rule.spawnPoint.position;

                if (instance.TryGetComponent<ItemObject>(out var itemObject))
                {
                    itemObject.SetItem(Item.CreateItemFrom(item));
                }
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
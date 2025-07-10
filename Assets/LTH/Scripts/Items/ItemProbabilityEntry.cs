using System;
using System.Collections.Generic;
using UnityEngine;

namespace LTH
{
    public enum SpawnConditionType
    {
        Used,              // 해당 아이템을 사용했으면 등장 금지
        Obtained,          // 해당 아이템을 얻었으면 등장 금지
        RequireObtained,   // 해당 아이템을 얻었을 때만 등장
        SingleSpawn        // 한 번만 등장
    }

    [Serializable]
    public class SpawnCondition
    {
        public SpawnConditionType conditionType;
        public string itemName;
    }

    [Serializable]
    public class ItemProbabilityEntry
    {
        public ItemGrade grade;

        [Range(0f, 1f)]
        public float probability;

        public List<ItemData> items;

        public List<SpawnCondition> spawnConditions = new();
    }
}
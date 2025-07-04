using System;
using System.Collections.Generic;
using UnityEngine;

namespace LTH
{
    [Serializable]
    public class ItemProbabilityEntry
    {
        public ItemGrade grade;

        [Range(0f, 1f)]
        public float probability;

        public List<ItemData> items;

        // 해당 아이템은 한 번만 등장
        public bool onlySpawnOnce;
        public string itemIdentifier;

        // 다음 아이템이 사용된 경우 스폰되지 않음
        public List<string> blockedIfUsedItem;
        // 다음 아이템을 획득한 경우 스폰되지 않음
        public List<string> blockedIfObtainedItem;
        // 다음 아이템을 획득한 경우에만 스폰됨
        public List<string> requiredObtainedItem;
    }
}
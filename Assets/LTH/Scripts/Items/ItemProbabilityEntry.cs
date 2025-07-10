using System;
using System.Collections.Generic;
using UnityEngine;

namespace LTH
{
    public enum SpawnConditionType
    {
        Used,              // �ش� �������� ��������� ���� ����
        Obtained,          // �ش� �������� ������� ���� ����
        RequireObtained,   // �ش� �������� ����� ���� ����
        SingleSpawn        // �� ���� ����
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
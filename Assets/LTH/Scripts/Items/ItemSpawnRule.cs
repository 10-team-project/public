using System;
using System.Collections.Generic;
using UnityEngine;

namespace LTH
{
    [Serializable]
    public class ItemSpawnRule
    {
        public Transform spawnPoint;
        public List<ItemProbabilityEntry> gradeEntries;

        public bool IsValid() // null üũ �� ��ȿ�� �˻�
        {
            return spawnPoint != null && gradeEntries != null && gradeEntries.Count > 0;
        }
    }
}
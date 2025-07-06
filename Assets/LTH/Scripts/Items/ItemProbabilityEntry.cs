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
    }
}


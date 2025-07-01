using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTJ
{
    [System.Serializable]
    public class GameData
    {
        public float hp;
        public float hunger;
        public float thirst;
        public float fatigue;
        public int day;

        public List<ItemSaveData> inventoryItems = new(); // ScriptableObject �̸� �Ǵ� ���� ID
        public List<string> quickSlotItemIDs = new();
    }
}
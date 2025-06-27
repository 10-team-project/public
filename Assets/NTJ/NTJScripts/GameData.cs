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

        public List<string> inventoryItemIDs = new(); // ScriptableObject 이름 또는 고유 ID
        public List<string> quickSlotItemIDs = new();
    }
}
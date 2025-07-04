using System.Collections;
using System.Collections.Generic;
using SHG;
using UnityEngine;

namespace LTH 
{
    [CreateAssetMenu(menuName = "Map/Ladder Place Point Data")]
    public class LadderPlacePointData : ScriptableObject
    {
        public GameObject Prefab;
        public EquipmentItemData[] RequiredItems;
    }
}
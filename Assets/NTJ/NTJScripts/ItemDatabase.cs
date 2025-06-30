using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTJ
{
    public static class ItemDatabase
    {
        private static Dictionary<string, ItemData> idToData;

        static ItemDatabase()
        {
            idToData = Resources.LoadAll<ItemData>("ItemDataFolder")
                .ToDictionary(x => x.ID, x => x); // ID 필드 필요
        }

        public static ItemData GetItemDataByID(string id)
        {
            return idToData.TryGetValue(id, out var data) ? data : null;
        }
    }
}
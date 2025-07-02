using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SHG;

namespace NTJ
{
    public static class ItemDatabase
    {
        private static Dictionary<string, ItemData> idToData;

        static ItemDatabase()
        {
            idToData = Resources.LoadAll<ItemData>(ItemStorageBase.ITEM_DIR)
                .ToDictionary(x => x.Id, x => x); // ID 필드 필요
        }

        public static ItemData GetItemDataByID(string id)
        {
            return idToData.TryGetValue(id, out var data) ? data : null;
        }

        public static Dictionary<string, ItemData>.Enumerator GetEnumerator()
        {
          return (idToData.GetEnumerator());
        }
    }
}

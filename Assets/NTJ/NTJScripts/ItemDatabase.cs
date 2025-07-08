using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SHG;

namespace NTJ
{
    public static class ItemDatabase
    {
        public static Dictionary<string, ItemData> idToData;

        static ItemDatabase()
        {
          idToData = ItemStorageBase.ALL_ITEMS;
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

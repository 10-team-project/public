using NTJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class NTJTestInven : MonoBehaviour
{
    public List<ItemSaveData> GetItemSaveDataList()
    {
      return (new());
//        return Items.Select(pair => new ItemSaveData
//        {
//            id = pair.Key.ID,
//            count = pair.Value
//        }).ToList();
    }

    public void LoadFromItemSaveDataList(List<ItemSaveData> list)
    {
//        Items.Clear();
//        foreach (var entry in list)
//        {
//            var data = ItemDatabase.GetItemDataByID(entry.id);
//            if (data != null)
//            {
//                Items[data] = entry.count;
//            }
//        }
    }
}

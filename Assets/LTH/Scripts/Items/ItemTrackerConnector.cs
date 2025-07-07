using System.Collections;
using System.Collections.Generic;
using SHG;
using UnityEngine;

namespace LTH
{
    /// <summary>
    /// ItemTracker에서 아이템 획득/사용 정보를 GameProgressManager에 자동 반영시킴
    /// App.cs => Awake()
    /// this.ItemTracker = new ItemTracker(this.Inventory);
    /// ItemTrackerConnector.ConnectToGameProgress(this.ItemTracker); => 추가하면 됨
    /// </summary>
 
    public class ItemTrackerConnector : MonoBehaviour
    {
       
        public static void ConnectToGameProgress(ItemTracker tracker)
        {
            tracker.OnChanged += t =>
            {
                foreach (var item in t.NewObtainedItems) // 최근에 획득된 아이템 리스트에 모든 아이템에 대해 하나씩 반복
                {
                    if (item != null && !string.IsNullOrEmpty(item.Name)) // 아이템이 null이 아니고, 아이템 이름도 null이나 빈 문자열이 아니면
                    {
                        GameProgressManager.Instance.MarkItemAsObtained(item.Name); // GameProgressManager에 이 아이템을 "획득했다"라고 기록한다
                    }         
                }

                foreach (var item in t.NewUsedItems)
                {
                    if (item != null && !string.IsNullOrEmpty(item.Name))
                    {
                        GameProgressManager.Instance.MarkItemAsUsed(item.Name);
                    }                        
                }
            };
        }
    }
}
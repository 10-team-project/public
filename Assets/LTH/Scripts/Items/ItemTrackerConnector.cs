using System.Collections;
using System.Collections.Generic;
using SHG;
using UnityEngine;

namespace LTH
{
    /// <summary>
    /// ItemTracker���� ������ ȹ��/��� ������ GameProgressManager�� �ڵ� �ݿ���Ŵ
    /// App.cs => Awake()
    /// this.ItemTracker = new ItemTracker(this.Inventory);
    /// ItemTrackerConnector.ConnectToGameProgress(this.ItemTracker); => �߰��ϸ� ��
    /// </summary>
 
    public class ItemTrackerConnector : MonoBehaviour
    {
       
        public static void ConnectToGameProgress(ItemTracker tracker)
        {
            tracker.OnChanged += t =>
            {
                foreach (var item in t.NewObtainedItems) // �ֱٿ� ȹ��� ������ ����Ʈ�� ��� �����ۿ� ���� �ϳ��� �ݺ�
                {
                    if (item != null && !string.IsNullOrEmpty(item.Name)) // �������� null�� �ƴϰ�, ������ �̸��� null�̳� �� ���ڿ��� �ƴϸ�
                    {
                        GameProgressManager.Instance.MarkItemAsObtained(item.Name); // GameProgressManager�� �� �������� "ȹ���ߴ�"��� ����Ѵ�
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
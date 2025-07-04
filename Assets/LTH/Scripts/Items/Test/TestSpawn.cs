using System.Collections;
using System.Collections.Generic;
using LTH;
using UnityEngine;

namespace LTH
{
    public class TestSpawn : MonoBehaviour
    {
        public ProbabilityItemSpawner spawner;

        [ContextMenu("Spawn All Items")]
        public void SpawnAll()
        {
            if (spawner != null)
            {
                spawner.SpawnAll();
                Debug.Log("������ ���� �Ϸ�");
            }
            else
            {
                Debug.LogWarning("spawner�� ����Ǿ� ���� �ʽ��ϴ�.");
            }
        }

        [ContextMenu("Use Book")]
        public void UseBook()
        {
            GameProgressManager.Instance.MarkItemAsUsed("BookTest1");
            Debug.Log("å ��� ó���� �� �ٽ� �������� ����");
        }

        [ContextMenu("Craft Radio")]
        public void CraftRadio()
        {
            GameProgressManager.Instance.MarkItemAsObtained("Radio");
            Debug.Log("���� ���� �Ϸ� ó���� (Radio ȹ���)");
        }

        [ContextMenu("Reset Progress")]
        public void ResetProgress()
        {
            GameProgressManager.Instance.LoadUsedItemList(new());
            GameProgressManager.Instance.LoadObtainedItemList(new());
            Debug.Log("���� ���� ���� �ʱ�ȭ");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using LTH;
using SHG;
using UnityEngine;

namespace LTH
{
    public class TestSpawn : MonoBehaviour
    {
        public ProbabilityItemSpawner spawner;
        public string ItemName1;
        public string ItemName2;

        [ContextMenu("Spawn All Items")]
        public void SpawnAll()
        {
            if (spawner != null)
            {
                spawner.SpawnAll();
                Debug.Log("아이템 스폰 완료");
            }
            else
            {
                Debug.LogWarning("spawner가 연결되어 있지 않습니다.");
            }
        }

        [ContextMenu("Use Book")]
        public void UseBook()
        {
            GameProgressManager.Instance.MarkItemAsUsed("ItemName1");
            Debug.Log("책 사용 처리됨 → 다시 스폰되지 않음");
        }

        [ContextMenu("Craft Radio")]
        public void CraftRadio()
        {
            GameProgressManager.Instance.MarkItemAsObtained("ItemName2");
            Debug.Log("라디오 제작 완료 처리됨 (Radio 획득됨)");
        }

        [ContextMenu("Reset Progress")]
        public void ResetProgress()
        {
            GameProgressManager.Instance.LoadUsedItemList(new());
            GameProgressManager.Instance.LoadObtainedItemList(new());
            Debug.Log("게임 진행 상태 초기화");
        }
    }
}

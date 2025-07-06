using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;


namespace LTH
{
    /// <summary>
    /// 게임 진행 상태를 관리하는 클래스입니다.
    /// 라디오 제작 여부와 1회성 아이템 사용 여부를 추적합니다.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("LTH/Game Progress Manager")]
    [RequireComponent(typeof(GameProgressManager))]

    public class GameProgressManager : SingletonBehaviour<GameProgressManager>
    {
        private HashSet<string> usedItemFlags = new();
        private HashSet<string> obtainedItemFlags = new();

        // 사용 처리 (ex. 책, 열쇠 등 1회성 아이템)
        public void MarkItemAsUsed(string id)
        {
            if (!string.IsNullOrEmpty(id))
                usedItemFlags.Add(id);
        }

        public bool IsItemUsed(string id)
        {
            return usedItemFlags.Contains(id);
        }

        // 획득 처리 (ex. 라디오를 제작)
        public void MarkItemAsObtained(string id)
        {
            if (!string.IsNullOrEmpty(id))
                obtainedItemFlags.Add(id);
        }

        public bool IsItemObtained(string id)
        {
            return obtainedItemFlags.Contains(id);
        }

        // 세이브/로드 연동용
        public List<string> GetUsedItemList() => new(usedItemFlags);
        public void LoadUsedItemList(List<string> list) => usedItemFlags = new(list);

        public List<string> GetObtainedItemList() => new(obtainedItemFlags);
        public void LoadObtainedItemList(List<string> list) => obtainedItemFlags = new(list);
    }
}
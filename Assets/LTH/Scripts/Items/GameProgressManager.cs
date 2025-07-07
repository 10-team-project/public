using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;


namespace LTH
{
    /// <summary>
    /// 게임의 핵심 진행 상태(예: 제작 완료, 주요 이벤트 완료 등)를 기록 및 조회합니다.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("LTH/Game Progress Manager")]
    [RequireComponent(typeof(GameProgressManager))]

    public class GameProgressManager : SingletonBehaviour<GameProgressManager>
    {
        // 아이템 획득 상태
        private HashSet<string> obtainedItemFlags = new();
        // 아이템 사용 상태 (ex. 열쇠, 책, 1회성 아이템 등)
        private HashSet<string> usedItemFlags = new();

        /// <summary>
        /// 아이템을 획득한 것으로 기록합니다.
        /// </summary>
        public void MarkItemAsObtained(string id)
        {
            if (!string.IsNullOrEmpty(id))
                obtainedItemFlags.Add(id);
        }

        /// <summary>
        /// 아이템을 사용한 것으로 기록합니다.
        /// </summary>
        public void MarkItemAsUsed(string id)
        {
            if (!string.IsNullOrEmpty(id))
                usedItemFlags.Add(id);
        }

        /// <summary>
        /// 특정 아이템을 획득했는지 확인합니다.
        /// </summary>
        public bool IsItemObtained(string id)
        {
            return obtainedItemFlags.Contains(id);
        }

        /// <summary>
        /// 특정 아이템을 사용했는지 확인합니다.
        /// </summary>
        public bool IsItemUsed(string id)
        {
            return usedItemFlags.Contains(id);
        }

        /// <summary>
        /// 저장용: 획득 상태 목록 반환
        /// </summary>
        public List<string> GetObtainedItemList() => new(obtainedItemFlags);

        /// <summary>
        /// 저장용: 사용 상태 목록 반환
        /// </summary>
        public List<string> GetUsedItemList() => new(usedItemFlags);

        /// <summary>
        /// 로딩 시: 획득 상태 복원
        /// </summary>
        public void LoadObtainedItemList(List<string> list)
        {
            obtainedItemFlags = new(list);
        }

        /// <summary>
        /// 로딩 시: 사용 상태 복원
        /// </summary>
        public void LoadUsedItemList(List<string> list)
        {
            usedItemFlags = new(list);
        }
    }
}
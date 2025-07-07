using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Patterns;
using UnityEngine;

/// <summary>
/// ***********************************************************************
/// 탈출 소켓 매니저
/// - 최대 3개의 아이템을 삽입할 수 있는 소켓 관리
/// - 아이템 삽입, 현재 삽입된 아이템 조회, 소켓 초기화 기능 제공
/// - 삽입된 아이템이 정답 아이템과 일치하는지 확인하는 기능 제공
/// ************************************************************************
/// </summary>

namespace LTH
{
    public class EscapeSocketManager : SingletonBehaviour<EscapeSocketManager>
    {
        [Tooltip("소켓 슬롯 최대 개수 (기본 3개)")]
        [SerializeField] private int maxSocketCount = 3;

        private List<ItemData> insertedItems = new();

        /// <summary>
        /// 아이템을 소켓에 삽입
        /// </summary>
        public bool TryInsertItem(ItemData item)
        {
            if (insertedItems.Count >= maxSocketCount) return false;


            insertedItems.Add(item);
            Debug.Log($"[Socket] 아이템 삽입됨: {item.Name}");
            return true;
        }

        /// <summary>
        /// 현재 삽입된 아이템 리스트 반환
        /// </summary>
        public List<ItemData> GetInsertedItems()
        {
            return new List<ItemData>(insertedItems);
        }

        /// <summary>
        /// 모든 슬롯 비우기
        /// </summary>
        public void ClearSockets()
        {
            insertedItems.Clear();
        }

        /// <summary>
        /// 현재 넣은 아이템이 정답 아이템과 일치하는지 확인
        /// </summary>
        public bool CheckEscapeSuccess()
        {
            var requiredItems = EscapeManager.Instance.GetRequiredEscapeItems();

            if (insertedItems.Count != requiredItems.Count)
                return false;

            var insertedSet = new HashSet<string>(insertedItems.Select(i => i.Id));
            var requiredSet = new HashSet<string>(requiredItems.Select(i => i.Id));

            bool result = insertedSet.SetEquals(requiredSet);
            Debug.Log($"[Socket] 탈출 조건 판정 결과: {(result ? "성공" : "실패")}");
            return result;
        }
    }
}

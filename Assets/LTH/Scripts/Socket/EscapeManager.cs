using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;

/// <summary>
/// ***********************************************************************
/// 게임 시작 시 랜덤으로 탈출 아이템 3개를 지정하고 저장해두는 매니저
/// List<ItemData> RequiredEscapeItems
/// void InitializeEscapeItems() ← 시작 시 호출
/// 정답 아이템을 외부(NPC 대사 등)에 제공할 수도 있음
/// ************************************************************************
/// </summary>

namespace LTH 
{
    public class EscapeManager : SingletonBehaviour<EscapeManager>
    {
        [Header("Escape Item List")]
        [SerializeField] private List<ItemData> escapeItemPool;

        public List<ItemData> RequiredEscapeItems { get; private set; } = new();

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            InitializeEscapeItems();
        }

        /// <summary>
        /// 게임 시작 시 탈출 아이템 3개를 무작위로 설정
        /// </summary>
        public void InitializeEscapeItems()
        {
            RequiredEscapeItems.Clear();

            if (escapeItemPool.Count < 3) return;

            List<ItemData> shuffled = new List<ItemData>(escapeItemPool);
            Shuffle(shuffled);

            for (int i = 0; i < 3; i++)
            {
                RequiredEscapeItems.Add(shuffled[i]);
            }

            Debug.Log("탈출 아이템 설정 완료: " +
                string.Join(", ", RequiredEscapeItems.ConvertAll(i => i.Name)));
        }

        public List<ItemData> GetRequiredEscapeItems()
        {
            return new List<ItemData>(RequiredEscapeItems);
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int rand = Random.Range(i, list.Count);
                (list[i], list[rand]) = (list[rand], list[i]);
            }
        }
    }
}
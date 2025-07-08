using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Patterns;
using UnityEngine;
using System;

namespace LTH 
{
    public class EscapeManager : SingletonBehaviour<EscapeManager>
    {
        [Header("Escape Item List")]
        [SerializeField] private List<ItemData> escapeItemPool;

        public List<ItemData> RequiredEscapeItems { get; private set; } = new();

        public event Action OnEscapeSuccess; // 탈출 성공 시 호출되는 이벤트
        public event Action OnEscapeFailure; // 탈출 실패 시 호출되는 이벤트

        public bool IsEscapeReady { get; private set; } = false;

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
        }

        /// <summary>
        /// 탈출 조건에 필요한 아이템 목록을 반환
        /// </summary>
        public List<ItemData> GetRequiredEscapeItems()
        {
            return new List<ItemData>(RequiredEscapeItems);
        }

        /// <summary>
        /// 수동 선택 아이템과 비교하여 탈출 성공 여부 판단
        /// </summary>
        public bool CheckEscapeSuccess(List<ItemData> selectedItems)
        {
            if (selectedItems.Count != RequiredEscapeItems.Count) return false;

            var selectedSet = new HashSet<string>(selectedItems.Select(i => i.Id));
            var requiredSet = new HashSet<string>(RequiredEscapeItems.Select(i => i.Id));

            return selectedSet.SetEquals(requiredSet);
        }

        public void EscapeSuccess()
        {
            IsEscapeReady = true;
            OnEscapeSuccess?.Invoke();
        }

        public void EscapeFailure()
        {
            IsEscapeReady = false;
            OnEscapeFailure?.Invoke();
        }

        /// <summary>
        /// 인벤토리 내 아이템이 탈출 조건을 만족하는지 판단 (이벤트는 호출하지 않음)
        /// </summary>
        public bool CheckInventoryForEscapeSuccess(List<ItemData> inventoryItems)
        {
            var requiredSet = new HashSet<string>(RequiredEscapeItems.Select(i => i.Id));
            var inventorySet = new HashSet<string>(inventoryItems.Select(i => i.Id));

            bool success = requiredSet.IsSubsetOf(inventorySet);

            if (success)
            {
                EscapeSuccess();
            }
            else
            {
                EscapeFailure();
            }

            return success;
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int rand = UnityEngine.Random.Range(i, list.Count);
                (list[i], list[rand]) = (list[rand], list[i]);
            }
        }
    }
}
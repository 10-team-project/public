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

        public event Action OnEscapeSuccess; // Ż�� ���� �� ȣ��Ǵ� �̺�Ʈ
        public event Action OnEscapeFailure; // Ż�� ���� �� ȣ��Ǵ� �̺�Ʈ

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            InitializeEscapeItems();
        }

        /// <summary>
        /// ���� ���� �� Ż�� ������ 3���� �������� ����
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

        public List<ItemData> GetRequiredEscapeItems()
        {
            return new List<ItemData>(RequiredEscapeItems);
        }

        public bool CheckEscapeSuccess(List<ItemData> selectedItems)
        {
            if (selectedItems.Count != RequiredEscapeItems.Count) return false;

            var selectedSet = new HashSet<string>(selectedItems.Select(i => i.Id));
            var requiredSet = new HashSet<string>(RequiredEscapeItems.Select(i => i.Id));

            return selectedSet.SetEquals(requiredSet);
        }

        public void EscapeSuccess()
        {
            OnEscapeSuccess?.Invoke();
        }

        public void EscapeFailure()
        {
            OnEscapeFailure?.Invoke();
        }

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
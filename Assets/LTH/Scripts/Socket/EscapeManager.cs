using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;

/// <summary>
/// ***********************************************************************
/// ���� ���� �� �������� Ż�� ������ 3���� �����ϰ� �����صδ� �Ŵ���
/// List<ItemData> RequiredEscapeItems
/// void InitializeEscapeItems() �� ���� �� ȣ��
/// ���� �������� �ܺ�(NPC ��� ��)�� ������ ���� ����
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

            Debug.Log("Ż�� ������ ���� �Ϸ�: " +
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
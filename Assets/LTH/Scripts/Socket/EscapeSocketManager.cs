using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Patterns;
using UnityEngine;

/// <summary>
/// ***********************************************************************
/// Ż�� ���� �Ŵ���
/// - �ִ� 3���� �������� ������ �� �ִ� ���� ����
/// - ������ ����, ���� ���Ե� ������ ��ȸ, ���� �ʱ�ȭ ��� ����
/// - ���Ե� �������� ���� �����۰� ��ġ�ϴ��� Ȯ���ϴ� ��� ����
/// ************************************************************************
/// </summary>

namespace LTH
{
    public class EscapeSocketManager : SingletonBehaviour<EscapeSocketManager>
    {
        [Tooltip("���� ���� �ִ� ���� (�⺻ 3��)")]
        [SerializeField] private int maxSocketCount = 3;

        private List<ItemData> insertedItems = new();

        /// <summary>
        /// �������� ���Ͽ� ����
        /// </summary>
        public bool TryInsertItem(ItemData item)
        {
            if (insertedItems.Count >= maxSocketCount) return false;


            insertedItems.Add(item);
            Debug.Log($"[Socket] ������ ���Ե�: {item.Name}");
            return true;
        }

        /// <summary>
        /// ���� ���Ե� ������ ����Ʈ ��ȯ
        /// </summary>
        public List<ItemData> GetInsertedItems()
        {
            return new List<ItemData>(insertedItems);
        }

        /// <summary>
        /// ��� ���� ����
        /// </summary>
        public void ClearSockets()
        {
            insertedItems.Clear();
        }

        /// <summary>
        /// ���� ���� �������� ���� �����۰� ��ġ�ϴ��� Ȯ��
        /// </summary>
        public bool CheckEscapeSuccess()
        {
            var requiredItems = EscapeManager.Instance.GetRequiredEscapeItems();

            if (insertedItems.Count != requiredItems.Count)
                return false;

            var insertedSet = new HashSet<string>(insertedItems.Select(i => i.Id));
            var requiredSet = new HashSet<string>(requiredItems.Select(i => i.Id));

            bool result = insertedSet.SetEquals(requiredSet);
            Debug.Log($"[Socket] Ż�� ���� ���� ���: {(result ? "����" : "����")}");
            return result;
        }
    }
}

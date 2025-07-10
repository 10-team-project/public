using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;


namespace LTH
{
    /// <summary>
    /// ������ �ٽ� ���� ����(��: ���� �Ϸ�, �ֿ� �̺�Ʈ �Ϸ� ��)�� ��� �� ��ȸ�մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("LTH/Game Progress Manager")]
    [RequireComponent(typeof(GameProgressManager))]

    public class GameProgressManager : SingletonBehaviour<GameProgressManager>
    {
        // ������ ȹ�� ����
        private HashSet<string> obtainedItemFlags = new();
        // ������ ��� ���� (ex. ����, å, 1ȸ�� ������ ��)
        private HashSet<string> usedItemFlags = new();

        /// <summary>
        /// �������� ȹ���� ������ ����մϴ�.
        /// </summary>
        public void MarkItemAsObtained(string id)
        {
            if (!string.IsNullOrEmpty(id))
                obtainedItemFlags.Add(id);
        }

        /// <summary>
        /// �������� ����� ������ ����մϴ�.
        /// </summary>
        public void MarkItemAsUsed(string id)
        {
            if (!string.IsNullOrEmpty(id))
                usedItemFlags.Add(id);
        }

        /// <summary>
        /// Ư�� �������� ȹ���ߴ��� Ȯ���մϴ�.
        /// </summary>
        public bool IsItemObtained(string id)
        {
            return obtainedItemFlags.Contains(id);
        }

        /// <summary>
        /// Ư�� �������� ����ߴ��� Ȯ���մϴ�.
        /// </summary>
        public bool IsItemUsed(string id)
        {
            return usedItemFlags.Contains(id);
        }

        /// <summary>
        /// �����: ȹ�� ���� ��� ��ȯ
        /// </summary>
        public List<string> GetObtainedItemList() => new(obtainedItemFlags);

        /// <summary>
        /// �����: ��� ���� ��� ��ȯ
        /// </summary>
        public List<string> GetUsedItemList() => new(usedItemFlags);

        /// <summary>
        /// �ε� ��: ȹ�� ���� ����
        /// </summary>
        public void LoadObtainedItemList(List<string> list)
        {
            obtainedItemFlags = new(list);
        }

        /// <summary>
        /// �ε� ��: ��� ���� ����
        /// </summary>
        public void LoadUsedItemList(List<string> list)
        {
            usedItemFlags = new(list);
        }
    }
}
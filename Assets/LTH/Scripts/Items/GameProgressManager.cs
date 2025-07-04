using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;


namespace LTH
{
    /// <summary>
    /// ���� ���� ���¸� �����ϴ� Ŭ�����Դϴ�.
    /// ���� ���� ���ο� 1ȸ�� ������ ��� ���θ� �����մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("LTH/Game Progress Manager")]
    [RequireComponent(typeof(GameProgressManager))]

    public class GameProgressManager : SingletonBehaviour<GameProgressManager>
    {
        private HashSet<string> usedItemFlags = new();
        private HashSet<string> obtainedItemFlags = new();

        // ��� ó�� (ex. å, ���� �� 1ȸ�� ������)
        public void MarkItemAsUsed(string id)
        {
            if (!string.IsNullOrEmpty(id))
                usedItemFlags.Add(id);
        }

        public bool IsItemUsed(string id)
        {
            return usedItemFlags.Contains(id);
        }

        // ȹ�� ó�� (ex. ������ ����)
        public void MarkItemAsObtained(string id)
        {
            if (!string.IsNullOrEmpty(id))
                obtainedItemFlags.Add(id);
        }

        public bool IsItemObtained(string id)
        {
            return obtainedItemFlags.Contains(id);
        }

        // ���̺�/�ε� ������
        public List<string> GetUsedItemList() => new(usedItemFlags);
        public void LoadUsedItemList(List<string> list) => usedItemFlags = new(list);

        public List<string> GetObtainedItemList() => new(obtainedItemFlags);
        public void LoadObtainedItemList(List<string> list) => obtainedItemFlags = new(list);
    }
}
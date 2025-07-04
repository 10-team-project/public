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
        public bool HasCraftedRadio { get; private set; }

        private HashSet<string> usedOneTimeItems = new();

        public void OnRadioCrafted()
        {
            HasCraftedRadio = true;
        }

        public void MarkItemAsUsed(string id)
        {
            if (!string.IsNullOrEmpty(id))
                usedOneTimeItems.Add(id);
        }

        public bool IsOneTimeItemUsed(string id)
        {
            return usedOneTimeItems.Contains(id);
        }

        // ���̺�/�ε� ����
        public List<string> GetUsedItemList() => new(usedOneTimeItems);
        public void LoadUsedItemList(List<string> list) => usedOneTimeItems = new(list);
        public void SetRadioCrafted(bool crafted) => HasCraftedRadio = crafted;
    }
}


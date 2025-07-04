using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;


namespace LTH
{
    /// <summary>
    /// 게임 진행 상태를 관리하는 클래스입니다.
    /// 라디오 제작 여부와 1회성 아이템 사용 여부를 추적합니다.
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

        // 세이브/로드 연동
        public List<string> GetUsedItemList() => new(usedOneTimeItems);
        public void LoadUsedItemList(List<string> list) => usedOneTimeItems = new(list);
        public void SetRadioCrafted(bool crafted) => HasCraftedRadio = crafted;
    }
}


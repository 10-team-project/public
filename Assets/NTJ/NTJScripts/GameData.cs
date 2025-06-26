using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTJ
{
    [System.Serializable]
    public class GameData
    {
        public int savedDay;
        public float playerHP;
        public float maxHP;

        // 플레이어 위치 저장
        public float playerPosX;
        public float playerPosY;

        // 인벤토리 (아이템 이름 리스트)
        public List<string> inventoryItems = new List<string>();
    }
}
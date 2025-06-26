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

        // �÷��̾� ��ġ ����
        public float playerPosX;
        public float playerPosY;

        // �κ��丮 (������ �̸� ����Ʈ)
        public List<string> inventoryItems = new List<string>();
    }
}
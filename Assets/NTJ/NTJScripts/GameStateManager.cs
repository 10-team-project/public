using System.Collections.Generic;
using UnityEngine;

namespace NTJ
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public float maxHP = 100f;
        public float playerHP = 100f;
        public List<string> inventory = new List<string>();
        public Transform playerTransform;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public GameData CreateSaveData(int day)
        {
            GameData data = new GameData
            {
                savedDay = day,
                playerHP = playerHP,
                maxHP = maxHP,
                // ��Ÿ ������ ������ �߰� ����
            };
            return data; // �� �� �־�� �մϴ�!
        }

        public void LoadFromData(GameData data)
        {
            if (data == null) return;

            playerHP = data.playerHP;
            maxHP = data.maxHP;
            inventory = new List<string>(data.inventoryItems);

            if (playerTransform != null && data.playerPosition.Length == 3)
            {
                playerTransform.position = new Vector3(
                    data.playerPosition[0],
                    data.playerPosition[1],
                    data.playerPosition[2]
                );
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace NTJ
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public float maxHP = 100f;
        public float playerHP = 100f;
        public List<string> inventoryItems = new List<string>();
        public Transform player;

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
                return;
            }

            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                    player = playerObj.transform;
                else
                    Debug.LogWarning("�÷��̾ ã�� �� �����ϴ�. �±׸� Ȯ�����ּ���.");
            }
        }

        public GameData CreateSaveData(int day)
        {
            GameData data = new GameData
            {
                savedDay = day,
                playerHP = playerHP,
                maxHP = maxHP,
                inventoryItems = new List<string>(inventoryItems),
                playerPosition = new float[] {
                player.position.x,
                player.position.y,
                player.position.z
            }
            };
            return data;
        }

        public void LoadFromData(GameData data)
        {
            if (data == null) return;

            playerHP = data.playerHP;
            maxHP = data.maxHP;
            inventoryItems = new List<string>(data.inventoryItems);
            player.position = new Vector3(
                data.playerPosition[0],
                data.playerPosition[1],
                data.playerPosition[2]
                );
        }
    }
}
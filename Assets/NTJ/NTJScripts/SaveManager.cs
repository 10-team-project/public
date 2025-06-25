using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NTJ
{
    public static class SaveManager
    {
        private static string SavePath => Application.persistentDataPath + "/save.json";

        public static void SaveGame(int currentDay)
        {
            GameData data = new GameData
            {
                savedDay = currentDay,
                playerHP = GameStateManager.Instance.playerHP,
                maxHP = GameStateManager.Instance.maxHP,  
                playerPosX = GameStateManager.Instance.playerPosition.x,
                playerPosY = GameStateManager.Instance.playerPosition.y,
                inventoryItems = new List<string>(GameStateManager.Instance.inventoryItems)
            };
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }

        public static void LoadGame()
        {
            if (!File.Exists(SavePath)) return;

            string json = File.ReadAllText(SavePath);
            GameData data = JsonUtility.FromJson<GameData>(json);

            GameStateManager.Instance.playerHP = data.playerHP;
            GameStateManager.Instance.maxHP = data.maxHP;
            GameStateManager.Instance.playerPosition = new Vector2(data.playerPosX, data.playerPosY);
            GameStateManager.Instance.inventoryItems = new List<string>(data.inventoryItems);

            GameTimeManager.InitialDay = data.savedDay;
        }

        public static bool HasSavedData()
        {
            return File.Exists(SavePath);
        }

        public static void ClearSave()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);
        }
    }
}
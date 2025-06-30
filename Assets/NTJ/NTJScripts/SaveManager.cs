using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NTJ
{
    public static class SaveManager
    {
        private static string SavePath => Application.persistentDataPath + "/save.json";

        public static void SaveData(GameData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }

        public static GameData LoadData()
        {
            if (!File.Exists(SavePath)) return null;

            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<GameData>(json);
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
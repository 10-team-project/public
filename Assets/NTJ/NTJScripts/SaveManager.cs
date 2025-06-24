using UnityEngine;

public static class SaveManager
{
    private const string SaveKey = "SavedDay";

    public static void SaveGame(int currentDay)
    {
        PlayerPrefs.SetInt(SaveKey, currentDay);
        PlayerPrefs.Save();
    }

    public static int LoadSavedDay() 
    {
        return PlayerPrefs.GetInt(SaveKey, 1); // ±âº»°ª: Day 1
    }

    public static bool HasSavedData()
    {
        return PlayerPrefs.HasKey(SaveKey);
    }

    public static void ClearSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
    }
}

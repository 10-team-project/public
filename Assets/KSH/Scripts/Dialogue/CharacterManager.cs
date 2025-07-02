using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterManager : MonoBehaviour
{
    private Dictionary<int, string> characterDict = new();
    private Dictionary<int, Sprite> portraitDict = new();
    
    private static CharacterManager instance;

    public static CharacterManager Instance
    {
        get {return instance;}
    }

    private void Awake()
    {
        if(instance == null)
            instance = this;

        LoadCharacterData("CSV/Character");
        LoadPortraitData("CSV/Portrait");

    }
    
    public Sprite GetPortraitData(string allId)
    {
        int portraitID = int.Parse(allId);
        portraitID %= 100;
        return portraitDict.TryGetValue(portraitID, out var sprite) ? sprite : null;
    }

    public string GetCharacterData(string allId)
    {
        int characterID = int.Parse(allId);
        characterID /= 100;
        return characterDict.TryGetValue(characterID, out var name) ? name : "???";
    }

    private void LoadCharacterData(string file)
    {
        var datas = CSVParser.Parse(file);
        foreach (var data in datas)
        {
            int id = Convert.ToInt32(data["ID"]);
            string name = data["Character"].ToString();
            characterDict[id] = name;
        }
    }

    private void LoadPortraitData(string file)
    {
        var datas = CSVParser.Parse(file);
        foreach (var data in datas)
        {
            int id = Convert.ToInt32(data["ID"]);
            string path = data["Portrait"].ToString();
            Sprite sprite = Resources.Load<Sprite>(path);
            if (sprite != null)
            {
                portraitDict[id] = sprite;
            }
        }
    }

    [System.Serializable]
    public class CharacterData
    {
        public int id;
        public string name;
    }
    
    [System.Serializable]
    public class PortraitData
    {
        public Sprite sprite;
        public int id;
    }
}

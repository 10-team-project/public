using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] CharacterData[] characterDatas; //캐릭터의 데이터
    
    private static CharacterManager instance;

    public static CharacterManager Instance
    {
        get {return instance;}
    }

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public CharacterData GetCharacterData(string n)
    {
        for (int i = 0; i < characterDatas.Length; i++)
        {
            if (characterDatas[i].name.Equals(n)) //만약 n이라는 이름을 가진 캐릭터가 있으면
            {
                return characterDatas[i]; //데이터 반환
            }
        }
        return null;
    }

    [System.Serializable]
    public class CharacterData
    {
        public Sprite sprite;
        public string name;
    }
}

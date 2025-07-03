using System;
using System.Collections;
using System.Collections.Generic;
using KSH;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject player1; //플레이어1 오브젝트
    [SerializeField] private GameObject player2; //플레이어2 오브젝트

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        CharacterType character = (CharacterType)PlayerPrefs.GetInt("PlayerSelect"); //Enum의 타입으로 데이터를 얻어온다.

        if (character == CharacterType.GangHana) //만약 강하나면
        {
            player1.SetActive(true); // player1 활성화
        }
        else if (character == CharacterType.Seonahyun)
        {
            player2.SetActive(true);
        }

    }
}    

using System;
using System.Collections;
using System.Collections.Generic;
using KSH;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        CharacterType character = (CharacterType)PlayerPrefs.GetInt("PlayerSelect");

        if (character == CharacterType.Character1)
        {
            player1.SetActive(true);
        }
        else if (character == CharacterType.Character2)
        {
            player2.SetActive(true);
        }

    }
}    

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTJ
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public float maxHP = 100f;
        public float playerHP = 100f;
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
    }
}
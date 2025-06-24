using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using Patterns;

namespace KSH
{
 public class AchieveManager : SingletonBehaviour<AchieveManager> //싱글톤
 {
     public GameObject LockChar;
     public GameObject UnlockChar;
     [Header("UnLockTest")]
     public bool istest = false;
 
     private const string Achieve = "Unlock";
     
     protected override void Awake()
     {
         base.Awake();
         DontDestroyOnLoad(gameObject);
         
         if (!PlayerPrefs.HasKey("MyData"))
         {
             Init();
         }
     }
 
     private void Init()
     {
         PlayerPrefs.SetInt("MyData", 1);
         PlayerPrefs.SetInt("Achieve", 0);
     }
 
     private void Start()
     {
         UnlockCharacter();
     }
 
     private void UnlockCharacter()
     {
         bool isUnlocked = PlayerPrefs.GetInt(Achieve) == 1; //Achieve의 키가 1이면 isUnlocked가 True
         LockChar.SetActive(!isUnlocked); //잠겨져있는 오브젝트는 false
         UnlockChar.SetActive(isUnlocked); //잠기지않은 오브젝트는 true
     }
 
     private void LateUpdate()
     {
         CheckAchieve();
     }
 
     private void CheckAchieve()
     {
         // bool isAcieved = 해금할 때 필요한 조건 : 엔딩보기 ;
 
         if (istest && PlayerPrefs.GetInt(Achieve) == 0) // && isAcieved //isAchieved가 true이고 Achieve의 키가 0이면
         {
             PlayerPrefs.SetInt(Achieve, 1); //1로 바꾸고
             UnlockCharacter(); //해금
         }
     }

     [Button("ClearPlayerPrefs(Test)")]
     private void ClearPlayerPrefs()
     {
         PlayerPrefs.DeleteAll();
     }
 }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using UnityEngine.SceneManagement;
namespace KSH
{
    public class SceneManager : SingletonBehaviour<AchieveManager> //싱글톤
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void MainGameScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Test");
        }
    }   
}

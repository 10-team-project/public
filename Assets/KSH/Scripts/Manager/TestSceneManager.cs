using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using UnityEngine.SceneManagement;
namespace KSH
{
    public class TestSceneManager : SingletonBehaviour<TestSceneManager> //싱글톤
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public AsyncOperation GameLoadScene(string sceneName)
        {
            return SceneManager.LoadSceneAsync(sceneName); //sceneName 씬으로 이동
        }
    }  
}

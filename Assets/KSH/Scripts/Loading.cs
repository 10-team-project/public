using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;
    public static string nextScene;

//    private IEnumerator Start()
//    {
//        if (string.IsNullOrEmpty(nextScene))
//        {
//            yield break;
//        }
//        
//        if (loadingBar == null) yield break;
//        loadingBar.value = 0f;
//        StartCoroutine(LoadScene());
//    }
//
//    public static void LoadScene(string sceneName)
//    {
//        nextScene = sceneName;
//        SceneManager.LoadScene("Loading");
//    }

    public IEnumerator LoadScene(string sceneName)
    {
        if (loadingBar == null) yield break;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
            if (loadingBar.value < 0.9f)
            {
                loadingBar.value = Mathf.MoveTowards(loadingBar.value, 0.9f, Time.deltaTime);
            }

            else if (op.progress >= 0.9f)
            {
                loadingBar.value = Mathf.MoveTowards(loadingBar.value, 1f, Time.deltaTime);
            }
            
            if(loadingBar.value >= 1f && op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }
        }
    }
}

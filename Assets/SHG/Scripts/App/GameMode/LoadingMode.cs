using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class LoadingMode : Singleton<LoadingMode>, IGameMode
  {
    public string SceneToLoad;
    public Action OnLoaded;
    public string SceneName => throw new NotImplementedException();
    GameObject loadingUI;

    public LoadingMode()
    {
      this.loadingUI = GameObject.Instantiate(Resources.Load<GameObject>("LoadingUI"));

      this.loadingUI.transform.SetParent(App.Instance.transform);
      this.loadingUI.SetActive(false);
    }

    public bool Equals(IGameMode other)
    {
      if (other is LoadingMode) {
        return (true);
      }
      return (false);
    }

    public IEnumerator OnEnd()
    {
      Debug.Log("LoadingState OnEnd");
      // TODO: 로딩 스크린 숨기기
      yield return (null);
      Debug.Log($"OnEnd {this.OnLoaded}");
      this.loadingUI.SetActive(false);
      this.OnLoaded?.Invoke();
      this.OnLoaded = null;
    }

    public IEnumerator OnStart()
    {
      Debug.Log("LoadingState OnStart");
      // TODO: 로딩 스크린 보여주기
      this.loadingUI.SetActive(true);
      yield return (new WaitForSeconds(2));
      yield return (null);
    }

    public void OnStartFromEditor()
    {
      Debug.Log("LoadingState OnStartFromEditor");
    }
  }
}

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

    public LoadingMode()
    {
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
      this.OnLoaded?.Invoke();
      this.OnLoaded = null;
      App.Instance.UIController.ShowQuickSlot();
    }

    public IEnumerator OnStart()
    {
      App.Instance.GameTimeManager.gameObject.SetActive(false);
      App.Instance.UIController.HideQuickSlot();
      Debug.Log("LoadingState OnStart");
      App.Instance.PlayerStatManager.HideUI();
      if (this.SceneToLoad != null) {
        yield return (App.Instance.SceneManager.GameLoadScene("Loading"));
        var loading = GameObject.Find("LoadingCanvas").GetComponent<Loading>();
        yield return (loading.LoadScene(this.SceneToLoad));
        
        //yield return (new WaitForSeconds(1));
        //var loadedScene = App.Instance.SceneManager.GameLoadScene(this.SceneToLoad);
        //while (!loadedScene.isDone) {
        //  yield return null;
        //}
        yield return (this.OnEnd());
      }
      else {
        yield return (this.OnEnd());
        Debug.LogWarning("No Scene to load in LoadingMode");
      }
    }

    public void OnStartFromEditor()
    {
      Debug.Log("LoadingState OnStartFromEditor");
    }
  }
}

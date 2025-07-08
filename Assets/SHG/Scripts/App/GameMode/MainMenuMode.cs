using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Patterns;
using UnityEngine.UI;
using NTJ;

namespace SHG
{
  public class MainMenuMode : Singleton<MainMenuMode>, IGameMode 
  {
    public string SceneName => "MainMenuScene";

    public bool Equals(IGameMode other)
    {
      if (other is MainMenuMode) {
        return (true);
      }
      return (false);
    }

    public void OnLoadSaveData()
    {
      
    }

    public IEnumerator OnEnd()
    {
      Debug.Log("MainMenuMode OnEnd");
      //TODO: 불러온 세이브 내용이 있을 경우 적용
      yield return (null);
    }

    public void OnClickSelectCharacter()
    {
      App.Instance.ChangeMode(GameMode.CharacterSelect, CharacterSelectMode.Instance.SceneName);
    }

    public IEnumerator OnStart()
    {
      Debug.Log("MainMenuMode OnStart");
      //TODO: 스플래시 스크린 보여주기
      //      메인 메뉴 BGM 재생
      //      세이브 목록 불러오기

      if (App.Instance.IsEditor && App.Instance.IsGamemodeControl &&
        SceneManager.GetActiveScene().name != "MainMenuScene") {
        var loadScene = App.Instance.SceneManager.GameLoadScene(this.SceneName);
        while (!loadScene.isDone) {
          yield return (null);
        }
      }

      //FIXME: Change to main menu script
      Button button = GameObject.Find("GameStartButton").GetComponent<Button>();
      button.onClick.AddListener(this.OnClickSelectCharacter);
      yield return (null);
    }

    public void OnStartFromEditor()
    {
      Debug.Log("MainMenuMode OnStartFromEditor load MainMenuScene\nTo disable Click: App > Gamemode control > disable ");
    }
  }
}

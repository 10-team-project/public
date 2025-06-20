using System.Collections;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class MainMenuMode : Singleton<MainMenuMode>, IGameMode 
  {
    public bool Equals(IGameMode other)
    {
      if (other is MainMenuMode) {
        return (true);
      }
      return (false);
    }

    public IEnumerator OnEnd()
    {
      Debug.Log("MainMenuMode OnEnd");
      //TODO: 불러온 세이브 내용이 있을 경우 적용
      yield return (null);
    }

    public IEnumerator OnStart()
    {
      Debug.Log("MainMenuMode OnStart");
      //TODO: 스플래시 스크린 보여주기
      //      메인 메뉴 scene 로드
      //      메인 메뉴 BGM 재생
      //      세이브 목록 불러오기
      yield return (null);
    }

    public void OnStartFromEditor()
    {
      Debug.Log("MainMenuMode OnStartFromEditor");
    }
  }
}

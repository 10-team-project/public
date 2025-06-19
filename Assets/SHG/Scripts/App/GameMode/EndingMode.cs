using System.Collections;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class EndingMode : Singleton<EndingMode>, IGameMode
  {
    public bool Equals(IGameMode other)
    {
      if (other is EndingMode) {
        return (true);
      }
      return (false);
    }

    public IEnumerator OnEnd()
    {
      Debug.Log("EndingMode OnEnd");
      // TODO: 게임 종료 또는 첫 화면으로 이동
      yield return (null);
    }

    public IEnumerator OnStart()
    {
      Debug.Log("OnStart EndingMode");
      // TODO: 게임 클리어 또는 게임 오버에 맞는 화면 보여주기   
      //       bgm 재생
      //       플레이 데이터 초기화
      yield return (null); 
    }

    public void OnStartFromEditor()
    {
      Debug.Log("EndingMode OnStartFromEditor");
    }
  }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class LoadingMode : Singleton<LoadingMode>, IGameMode
  {
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
    }

    public IEnumerator OnStart()
    {
      Debug.Log("LoadingState OnStart");
      // TODO: 로딩 스크린 보여주기
      yield return (null);
    }

    public void OnStartFromEditor()
    {
      Debug.Log("LoadingState OnStartFromEditor");
    }
  }
}

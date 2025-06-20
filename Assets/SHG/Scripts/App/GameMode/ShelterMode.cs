using System.Collections;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class ShelterMode :Singleton<ShelterMode>, IGameMode
  {
    public bool Equals(IGameMode other)
    {
      if (other is ShelterMode) {
        return (true);
      }
      return (false);
    }

    public IEnumerator OnEnd()
    {
      Debug.Log("ShelterMode OnEnd");
      yield return (null);
    }

    public IEnumerator OnStart()
    {
      Debug.Log("ShelterMode OnStart");
      // TODO: 필요시 로딩 보여주기
      //       방공호 Scene 로드
      //       BGM 재생
      //       필요할 경우 튜토리얼 보여주기
      yield return (null);
    }

    public void OnStartFromEditor()
    {
      Debug.Log("ShelterMode OnStartFromEditor");
      //TODO: Scriptable object를 이용해서 미리 설정된 값 가져오기
    }
  }
}


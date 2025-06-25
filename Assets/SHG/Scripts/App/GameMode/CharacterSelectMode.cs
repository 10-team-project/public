using System.Collections;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class CharacterSelectMode : Singleton<CharacterSelectMode>, IGameMode
  {

    public string SceneName => "CharacterChoiceScene";

    public bool Equals(IGameMode other)
    {
      if (other is CharacterSelectMode) {
        return (true);
      }
      return (false);
    }

    public void OnSelectCharacter(Character newCharacter)
    {
      //TODO: 캐릭터 데이터 저장, 다음 scene 로드      
      App.Instance.ChangeMode(GameMode.Shelter, ShelterMode.Instance.SceneName);
    }

    public IEnumerator OnEnd()
    {
      Debug.Log("CharacterSelectMode OnEnd");
      // TODO: 선택한 캐릭터 확인
      //       캐릭터 데이터 불러오기
      yield return (null);
    }

    public IEnumerator OnStart()
    {
      Debug.Log("CharacterSelectMode OnStart");
      // TODO: 선택 가능한 캐릭터 보여주기
      //       bgm 재생
      yield return (null);
    }

    public void OnStartFromEditor()
    {
      Debug.Log("CharacterSelectMode OnStartFromEditor");
    }
  }
}

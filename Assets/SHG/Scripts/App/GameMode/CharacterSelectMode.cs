using System.Collections;
using UnityEngine;
using Patterns;
using KSH;

namespace SHG
{
  using Character = TempCharacter;
  public class CharacterSelectMode : Singleton<CharacterSelectMode>, IGameMode
  {
    GameObject[] CharacterPrefabs;
    GameObject[] NpcPrefabs;
    public GameObject CharacterPrefab { get; private set; }
    public GameObject NpcPrefab { get; private set; }
    int selectedCharacter;

    public CharacterSelectMode()
    {
      this.CharacterPrefabs = new GameObject[]{
        Resources.Load<GameObject>("Alice_Player"),
        Resources.Load<GameObject>("Arisa_Player"),
      };
      this.NpcPrefabs = new GameObject[] {
        Resources.Load<GameObject>("Alice_NPC"),
        Resources.Load<GameObject>("Arisa_NPC"),
      };
    }

    public string SceneName => "CharacterChoiceScene";

    public bool Equals(IGameMode other)
    {
      if (other is CharacterSelectMode) {
        return (true);
      }
      return (false);
    }

    public void OnSelectCharacter(int characterIndex)
    {
      this.selectedCharacter = characterIndex;
      this.CharacterPrefab = this.CharacterPrefabs[characterIndex];
      this.NpcPrefab = characterIndex == 0 ?
        this.NpcPrefabs[1] : this.NpcPrefabs[0];
      //TODO: 캐릭터 데이터 저장, 다음 scene 로드      
      App.Instance.ChangeMode(GameMode.Shelter,
        ShelterMode.Instance.SceneName);
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

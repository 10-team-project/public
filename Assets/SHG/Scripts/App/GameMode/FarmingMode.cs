using System;
using System.Collections;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class FarmingMode : Singleton<FarmingMode>, IGameMode
  {
    public string SceneName => "FieldTest";

    public bool Equals(IGameMode other)
    {
      if (other is FarmingMode) {
        return (true);
      }
      return (false);
    }

    public IEnumerator OnEnd()
    {
      Debug.Log("FarmingMode OnEnd");
      yield return (null);
    }

    public IEnumerator OnStart()
    {
      Debug.Log("FarmingMode OnStart");
      // TODO: 필요하다면 로딩 화면 보여주기
      //       현재 상황에 맞는 scene 불러오기
      //       적절한 곳에 아이템 배치하기 

      var gates = GameObject.FindObjectsOfType<MapGate>();
      foreach (var gate in gates) {
        gate.OnMove += this.OnEnterToShelterGate; 
      }
      yield return (null);
    }

    void OnEnterToShelterGate(string sceneName)
    {
      App.Instance.ChangeMode(GameMode.Shelter, sceneName);
    }

    public void OnStartFromEditor()
    {
      Debug.Log("FarmingMode OnStartFromEditor");
      // TODO: 테스트 할 수 있는 위치에 아이템 배치하기
      //ItemSpawnTest itemSpawnTester = GameObject.Find("ItemSpawn").GetComponent<ItemSpawnTest>();
      //itemSpawnTester.SpawnItem(3);
    }
  }
}

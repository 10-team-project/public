using System;
using System.Collections;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class FarmingMode : Singleton<FarmingMode>, IGameMode
  {
    public string SceneName => "FieldTest";
    public GameScene CurrentScene;

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
      App.Instance.GameTimeManager.gameObject.SetActive(false);
      App.Instance.PlayerStatManager.HideUI();
      yield return (null);
    }

    public IEnumerator OnStart()
    {
      App.Instance.GameTimeManager.gameObject.SetActive(false);
      Debug.Log("FarmingMode OnStart");
      // TODO: 필요하다면 로딩 화면 보여주기
      //       현재 상황에 맞는 scene 불러오기
      //       적절한 곳에 아이템 배치하기 

      var gates = GameObject.FindObjectsOfType<MapGate>();
      foreach (var gate in gates) {
        gate.OnMove += this.OnEnterToShelterGate; 
      }
      GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
      var player = GameObject.Instantiate(App.Instance.CharacterPrefab);
      if (spawnPoints.Length == 1) {
        player.transform.position = spawnPoints[0].transform.position;
      }
      else {
        foreach (var point in spawnPoints) {
          if (point.name == this.CurrentScene.Name) {
            Debug.Log(point.name);
            player.transform.position = point.transform.position;
            break;
          } 
        }
      }
      GameObject[] teleportPoints = GameObject.FindGameObjectsWithTag("TeleportPoint");
      foreach (var point in teleportPoints) {
        if (point.name != this.CurrentScene.Name) {
          point.gameObject.SetActive(false);
        } 
      }
      App.Instance.CameraController.Player = player.transform;
      App.Instance.CameraController.gameObject.SetActive(true);
      App.Instance.PlayerStatManager.ShowUI();
      App.Instance.GameTimeManager.gameObject.SetActive(true);
      yield return (null);
    }

    void OnEnterToShelterGate(string sceneName)
    {
      App.Instance.ChangeMode(GameMode.Shelter, sceneName);
    }

    public void OnStartFromEditor()
    {
      Debug.Log("FarmingMode OnStartFromEditor");
    }
  }
}

using System.Collections;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class ShelterMode :Singleton<ShelterMode>, IGameMode
  {
    public string SceneName => "Classroom";
    MapGate gate;

    public bool Equals(IGameMode other)
    {
      if (other is ShelterMode) {
        return (true);
      }
      return (false);
    }

    public IEnumerator OnEnd()
    {
      App.Instance.GameTimeManager.gameObject.SetActive(false);
      App.Instance.PlayerStatManager.HideUI();
      yield return (null);
    }

    public void OnEnterFarmingGate(GameScene scene)
    {
      FarmingMode.Instance.CurrentScene = scene;
      App.Instance.ChangeMode(GameMode.Farming, scene.FileName);
    }

    public IEnumerator OnStart()
    {
      GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
      GameObject player = null;
      foreach (var point in spawnPoints) {
        if (point.name == "PlayerSpawnPoint") {
          player = GameObject.Instantiate(
            App.Instance.CharacterPrefab);
          player.transform.position = point.transform.position;
        } 
        else if (point.name == "NpcSpawnPoint") {
          var npc = GameObject.Instantiate(
            App.Instance.NpcPrefab);
         npc.transform.position = point.transform.position;
        }
      }
      //App.Instance.GameTimeManager.gameObject.SetActive(true);
      App.Instance.PlayerStatManager.ShowUI();
      App.Instance.CameraController.Player = player.transform;
      App.Instance.CameraController.gameObject.SetActive(true);
      this.gate = GameObject.Find("Gate").GetComponent<MapGate>();
      yield return (null);
    }

    public void SetGateDest(string name)
    {
      var scene = GameModeManager.Instance.Scenes[name];
      this.gate.SetScene(scene);
    }

    public void OnStartFromEditor()
    {
      Debug.Log("ShelterMode OnStartFromEditor");
      //TODO: Scriptable object를 이용해서 미리 설정된 값 가져오기
    }
  }
}


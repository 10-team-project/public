using System.Collections;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class ShelterMode :Singleton<ShelterMode>, IGameMode
  {
    public string SceneName => "Classroom";
    MapGate gate;
    bool IsEventTriggerable;

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
      App.Instance.GameTimeManager.OnDayChanged -= this.OnDayChanged;
      this.UnRegisterEvent();
      yield return (null);
    }

    public void OnEnterFarmingGate(GameScene scene)
    {
      FarmingMode.Instance.CurrentScene = scene;
      App.Instance.ChangeMode(GameMode.Farming, scene.FileName);
      App.Instance.PlayerStatManager.Fatigue.Resource.Decrease(50f);
      // TODO: Fatigue
    }

    public IEnumerator OnStart()
    {
      this.IsEventTriggerable = true;
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
      App.Instance.GameTimeManager.player = player.transform;
      App.Instance.GameTimeManager.OnDayChanged += this.OnDayChanged;
      App.Instance.GameTimeManager.gameObject.SetActive(true);
      App.Instance.PlayerStatManager.ShowUI();
      App.Instance.CameraController.Player = player.transform;
      App.Instance.CameraController.gameObject.SetActive(true);
      this.gate = GameObject.Find("Gate").GetComponent<MapGate>();
      yield return (null);
      this.HandleGameEvent();
    }

    void OnDayChanged(int newDay) 
    {
      this.IsEventTriggerable = true; 
      this.HandleGameEvent();
    }

    void OnEventStart(GameEvent gameEvent)
    {
      if (this.IsEventTriggerable) {
        Debug.Log(gameEvent.Name);
        this.IsEventTriggerable = false;
      }
    }

    void HandleGameEvent()
    {
      var currentEvents = App.Instance.GameEventHandler.EventCandidates;
      if (currentEvents.Count > 0) {
        if (currentEvents.Count == 1) {
          App.Instance.GameEventHandler.TriggerEvent(currentEvents[0]);
        }
        else {
          int priority = int.MaxValue;
          GameEvent eventToTrigger = null;
          foreach (var e in App.Instance.GameEventHandler.EventCandidates) {
            
            if (e is StoryGameEvent storyGameEvent) {
              if (eventToTrigger == null || 
                priority > storyGameEvent.Priority) {
                priority = storyGameEvent.Priority;
                eventToTrigger = storyGameEvent;
              }
            }
            else if (e is NormalGameEvent normalGameEvent &&
              eventToTrigger == null) {
              eventToTrigger = normalGameEvent;
            }
          }
          if (eventToTrigger != null) {
            App.Instance.GameEventHandler.TriggerEvent(eventToTrigger);
          }
        }
      }
      else {
        App.Instance.GameEventHandler.IsEventTriggerable = true;
        App.Instance.GameEventHandler.OnNormalEventStart += this.OnEventStart;
        App.Instance.GameEventHandler.OnStoryEventStart += this.OnEventStart;
      }
      App.Instance.GameEventHandler.ClearEventCandiates();
    }

    void UnRegisterEvent()
    {
      App.Instance.GameEventHandler.OnNormalEventStart -= this.OnEventStart;
      App.Instance.GameEventHandler.OnStoryEventStart -= this.OnEventStart;

      App.Instance.GameEventHandler.IsEventTriggerable = false;
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


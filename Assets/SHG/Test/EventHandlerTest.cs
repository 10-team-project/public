using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class EventHandlerTest : MonoBehaviour
  {

    [SerializeField, ReadOnly]
    GameEvent startedEvent;  

    [SerializeField][ReadOnly]
    bool trigger;

    void Start()
    {
      this.ToggleTrigger();
    }

    [Button ("Clear")]
    void Clear()
    {
      this.startedEvent = null;
    }

    [Button ("Toggle trigger")]
    void ToggleTrigger()
    {
      this.trigger = !this.trigger;
      if (this.trigger) {
        App.Instance.GameEventHandler.OnNormalEventStart += this.OnEventStart;
        App.Instance.GameEventHandler.OnStoryEventStart += this.OnEventStart;
      }
      else {
        App.Instance.GameEventHandler.OnNormalEventStart -= this.OnEventStart;
        App.Instance.GameEventHandler.OnStoryEventStart -= this.OnEventStart;
      }
      App.Instance.GameEventHandler.IsEventTriggerable = this.trigger;
    }

    [Button ("Test date change")]
    void TestDateChange(int date)
    {
      //App.Instance.GameEventHandler.OnDateChanged(date);
    }

    [Button ("Test resource change")]
    void TestResouceChange(float oldValue, float newValue, TempCharacter.Stat stat)
    {
      //App.Instance.GameEventHandler.OnResourceChanged(stat, oldValue, newValue);
    }

    [Button ("Print events")]
    void PrintEvents()
    {
      var index = 1;
      Debug.Log("events");
      foreach (var gameEvent in App.Instance.GameEventHandler.EventCandidates) {
        Debug.Log($"{index++}: {gameEvent.Name}");     
      }
      App.Instance.GameEventHandler.ClearEventCandiates();
    }

    void OnEventStart(GameEvent gameEvent)
    {
      Debug.Log($"{gameEvent.Name} start");
      this.startedEvent = gameEvent;
    }
  }
}

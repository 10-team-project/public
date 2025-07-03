using System;
using UnityEngine;
using System.Collections.Generic;

namespace SHG
{
  using Character = TempCharacter;
  public class GameEventHandler
  {
    const string EVENT_DIR = "Assets/SHG/Test/Events";

    List<GameEvent> storyEvents;
    List<GameEvent> normalEvents;
    Dictionary<ItemData, GameEvent> eventsByItemTrigger;
    Dictionary<int, GameEvent> eventsByDateTrigger;
    Dictionary<Character.Stat, List<GameEvent>> eventsByStatTrigger; 
    Dictionary<string, GameEvent> eventsByName;

    bool TryFindStoryEvent(out NormalGameEvent result, Func<GameEvent, bool> query)
    {
      foreach (var storyEvent in this.storyEvents) {
        if (query(storyEvent)) {
          result = storyEvent as NormalGameEvent;
          return (true);
        } 
      }
      result = null;
      return (false);
    }

    bool TryFindEventByName(in string name, out GameEvent result) 
    {
      if (this.eventsByName.TryGetValue(name, out GameEvent found)) {
        result = found;
        return (true);
      }
      result = null;
      return (false);
    }

    //bool TryFindEventByDateTrigger(int date, out GameEvent)

    public GameEventHandler()
    {
      this.storyEvents = new ();
      this.normalEvents = new ();
      this.eventsByName = new ();
      this.eventsByItemTrigger = new ();
      this.eventsByDateTrigger = new ();
      this.eventsByStatTrigger = new Dictionary<Character.Stat, List<GameEvent>> {
        {Character.Stat.Hp, new () },
        {Character.Stat.Fatigue, new ()},
        {Character.Stat.Hunger, new () },
        {Character.Stat.Hydration, new ()}
      };
      this.LoadAllEvents();
    }

    void LoadAllEvents()
    {
      var allEvents = Utils.LoadAllFrom<GameEvent>(EVENT_DIR);
      foreach (var gameEvent in allEvents) {
        Debug.Log(gameEvent.Name); 
        this.eventsByName.TryAdd(gameEvent.Name, gameEvent);
        foreach (var reward in gameEvent.Rewards) {
          Debug.Log(reward); 
        }
        if (gameEvent is StoryGameEvent normalGameEvent) {
          this.AddNormalGameEvent(normalGameEvent);
        }
        else if (gameEvent is NormalGameEvent storyGameEvent) {
          this.AddStoryGameEvent(storyGameEvent);
        }
      }
    }

    void AddNormalGameEvent(StoryGameEvent gameEvent)
    {
      this.normalEvents.Add(gameEvent);
      switch (gameEvent.Trigger) {
        case DateChangeTrigger dateTrigger:
          this.eventsByDateTrigger.TryAdd(dateTrigger.Date, gameEvent);
          break;
        case NewItemTrigger itemTrigger:
          this.eventsByItemTrigger.TryAdd(itemTrigger.Item, gameEvent);
          break;
        case ResourceChangeTrigger resourceTrigger:
          this.eventsByStatTrigger[resourceTrigger.Stat].Add(gameEvent);
          break;
        default:
          throw (new NotImplementedException());
      }
    }

    void AddStoryGameEvent(NormalGameEvent gameEvent)
    {
      this.storyEvents.Add(gameEvent);
    }
  }
}

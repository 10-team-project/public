using System;
using System.Collections.Generic;
using KSH;
using NTJ;
using UnityEngine;

namespace SHG
{
  using Character = TempCharacter;
  public class GameEventHandler
  {
    const string EVENT_DIR = "Assets/LGJ/Events";
    public bool IsEventTriggerable;
    public Action<StoryGameEvent> OnStoryEventStart;
    public Action<NormalGameEvent> OnNormalEventStart;

    List<GameEvent> storyEvents;
    List<GameEvent> normalEvents;
    Dictionary<ItemData, StoryGameEvent> eventsByItemTrigger;
    Dictionary<int, StoryGameEvent> eventsByDateTrigger;
    Dictionary<Character.Stat, List<StoryGameEvent>> eventsByStatTrigger; 
    Dictionary<string, GameEvent> eventsByName;
    public List<GameEvent> EventCandidates { get; private set; }

    public void ClearEventCandiates()
    {
      this.EventCandidates.Clear();
    }

    public void TriggerEvent(GameEvent gameEvent)
    {
      if (gameEvent is NormalGameEvent normalEvent) {
        this.OnNormalEventStart?.Invoke(normalEvent);
      }
      else if (gameEvent is StoryGameEvent storyEvent) {
        this.OnStoryEventStart?.Invoke(storyEvent);
      }
    }

    public bool TryFindStoryEvent(out NormalGameEvent result, Func<GameEvent, bool> query)
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

    public bool TryFindEventByName(in string name, out GameEvent result) 
    {
      if (this.eventsByName.TryGetValue(name, out GameEvent found)) {
        result = found;
        return (true);
      }
      result = null;
      return (false);
    }

    public bool TryFindEventByDateTrigger(int date, out GameEvent result)
    {
      if (this.eventsByDateTrigger.TryGetValue(date, out StoryGameEvent found)) {
        result = found;
        return (true);
      }
      result = null;
      return (false);
    }
    
    public bool TryFindEventByItem(ItemData item, out GameEvent result)
    {
      if (this.eventsByItemTrigger.TryGetValue(item, out StoryGameEvent found))
      {
        result = found;
        return (true);
      }
      result = null;
      return (false);
    }

    public GameEventHandler()
    {
      this.storyEvents = new ();
      this.normalEvents = new ();
      this.eventsByName = new ();
      this.EventCandidates = new ();
      this.eventsByItemTrigger = new ();
      this.eventsByDateTrigger = new ();
      this.eventsByStatTrigger = new Dictionary<Character.Stat, List<StoryGameEvent>> {
        {Character.Stat.Hp, new () },
        {Character.Stat.Fatigue, new ()},
        {Character.Stat.Hunger, new () },
        {Character.Stat.Hydration, new ()}
      };
      this.LoadAllEvents();
      this.OnStoryEventStart += this.StartScript;
      this.OnNormalEventStart += this.StartScript;
    }

    void StartScript(GameEvent gameEvent)
    {
      if (int.TryParse(gameEvent.Name, out int id)) {
        ScriptManager.Instance.StartScript(id);
      }
    }

    public void RegisterItemTracker(ItemTracker itemTracker)
    {
      itemTracker.OnChanged += this.OnItemTrackerChanged;
    }

    public void RegisterStatTracker(PlayerStatManager playerStat) {
      playerStat.HP.Resource.OnResourceChanged += (_, oldValue, newValue) => this.OnResourceChanged(
        TempCharacter.Stat.Hp,
        oldValue, 
        newValue
        );
      playerStat.Fatigue.Resource.OnResourceChanged += (_, oldValue, newValue) => this.OnResourceChanged(
        TempCharacter.Stat.Fatigue,
        oldValue, 
        newValue
        );
      playerStat.Thirsty.Resource.OnResourceChanged += (_, oldValue, newValue) => this.OnResourceChanged(
        TempCharacter.Stat.Hydration,
        oldValue,
        newValue
        );
      playerStat.Hunger.Resource.OnResourceChanged += (_, oldValue, newValue) => this.OnResourceChanged(
        TempCharacter.Stat.Hunger,
        oldValue,
        newValue
        );
    }

    public void RegisterGameTimeTracker(GameTimeManager gameTimeManager) {
      gameTimeManager.OnDayChanged += this.OnDateChanged; 
    }

    void OnItemsObtained(List<ItemData> items)
    {
      foreach (var item in items) {
        this.OnObtainItem(item); 
      } 
    }

    void OnItemTrackerChanged(ItemTracker tracker)
    {
      if (tracker.NewObtainedItems.Count > 0) {
        tracker.ConsumeNewObtainedItems(this.OnItemsObtained);
      }
    }

    void LoadAllEvents()
    {
      var allEvents = Utils.LoadAllFrom<GameEvent>(EVENT_DIR);
      foreach (var gameEvent in allEvents) {
        this.eventsByName.TryAdd(gameEvent.Name, gameEvent);
        if (gameEvent is StoryGameEvent normalGameEvent) {
          this.AddNormalGameEvent(normalGameEvent);
        }
        else if (gameEvent is NormalGameEvent storyGameEvent) {
          this.AddStoryGameEvent(storyGameEvent);
        }
      }
    }

    void OnDateChanged(int date)
    {
      if (this.TryFindEventByDateTrigger(date, out GameEvent gameEvent)) {
        this.OnFoundEventByTrigger(gameEvent);
        return ;
      } 
    }

    void OnResourceChanged(
      Character.Stat stat, 
      float oldValue,
      float newValue)
    {
      if (oldValue != newValue) {
        if (this.eventsByStatTrigger.TryGetValue(stat, out List<StoryGameEvent> events)) {
          foreach (var gameEvent in events) {
            var trigger = gameEvent.Trigger as ResourceChangeTrigger;
            if (this.IsInTriggerRange(trigger, stat, oldValue, newValue)) {
              this.OnFoundEventByTrigger(gameEvent);
            }
          }
        }
      }
    }

    bool IsInTriggerRange(ResourceChangeTrigger trigger, Character.Stat stat, float oldValue, float newValue)
    {
      ChangeTrend trend = oldValue > newValue ? ChangeTrend.Decrease: ChangeTrend.Increase;
      if (trigger.Stat != stat &&
            trigger.Trend != trend ) {
        return (false);
      }
      switch (trend) {
        case (ChangeTrend.Decrease):
          return (oldValue + float.Epsilon >= trigger.Value &&
            newValue - float.Epsilon <= trigger.Value);
        case (ChangeTrend.Increase):
          return (oldValue - float.Epsilon <= trigger.Value &&
            newValue + float.Epsilon >= trigger.Value);
        default: 
          throw (new NotImplementedException());
      }
    }

    void OnObtainItem(ItemData item)
    {
      if (this.eventsByItemTrigger.TryGetValue(item, out StoryGameEvent gameEvent)) {
        this.OnFoundEventByTrigger(gameEvent);  
      }
    }

    void OnFoundEventByTrigger(GameEvent gameEvent)
    {
      Debug.LogWarning($"new event triggered {gameEvent.Name}");
      if (this.IsEventTriggerable) {
        if (gameEvent.IsStoryEvent) {
          this.OnStoryEventStart?.Invoke(gameEvent as StoryGameEvent);
        } 
        else {
          this.OnNormalEventStart?.Invoke(gameEvent as NormalGameEvent);
        }
      }
      else {
        this.EventCandidates.Add(gameEvent);
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

using EditorAttributes;
using Void = EditorAttributes.Void;

namespace SHG
{
  public abstract class GameEvent : IdentifiableScriptableObject
  {
    public string Name => this.eventName;
    public GameEventReward[] Rewards;
    public string[] Prolouge => this.startMessages;

    [Validate("Event name is empty", nameof(IsNameEmpty), MessageMode.Error)]
    string eventName;
    [Validate("Event reward is empty", nameof(IsRewardEmpty), MessageMode.Warning)]
    GameEventReward[] eventRewards;
    [Validate("Some event reward is none", nameof(HasNullReward), MessageMode.Error)]
    Void nullRewardCheck;
    [Validate("Empty start message", nameof(IsStartMessageEmpty), MessageMode.Warning)]
    string[] startMessages;
    [Validate("Some starte message is none", nameof(HasNullStartMessage), MessageMode.Error)]
    Void nullStartMessageCheck;

    protected bool IsNameEmpty() => (this.eventName == null || this.eventName.Replace(" ", "").Length == 0);
    protected bool IsRewardEmpty() => (this.eventRewards == null || this.eventRewards.Length == 0);
    protected bool HasNullReward()
    {
      if (this.IsRewardEmpty()) {
        return (false);
      }
      for (int i = 0; i < this.Rewards.Length; i++) {
        if (this.Rewards[i] == null) {
          return (true);
        } 
      }
      return (false);
    }

    protected bool IsStartMessageEmpty() => (this.startMessages == null || this.startMessages.Length == 0);
    protected bool HasNullStartMessage()
    {
      if (this.IsStartMessageEmpty()) {
        return (false);
      }
      for (int i = 0; i < this.Prolouge.Length; i++) {
        if (this.Prolouge[i] == null || this.Prolouge[i].Replace(" ", "").Length == 0) {
          return (true);
        } 
      }
      return (false);
    }
  }
}

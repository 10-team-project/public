using System;
using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

namespace SHG
{
  [Serializable]
  [CreateAssetMenu (menuName = "ScriptableObjects/Event/Hint Reward")]
  public class HintReward : GameEventReward
  {
    public string[] Messages => this.hintMessages;
    [Validate("Empty messages", nameof(HasEmptyMessage), MessageMode.Error)]
    string[] hintMessages;
    [Validate("Some message is none", nameof(HasInvalidMessage), MessageMode.Error)]
    Void nullMessageCheck;

    protected bool HasEmptyMessage() => (this.Messages == null || this.Messages.Length == 0);
    protected bool HasInvalidMessage()
    {
      if (this.HasEmptyMessage()) {
        return (false);
      }
      for (int i = 0; i < this.Messages.Length; i++) {
        if (this.Messages[i] == null || this.Messages[i].Replace(" ", "").Length == 0) {
          return (true);
        } 
      }
      return (false);
    }
  }
}

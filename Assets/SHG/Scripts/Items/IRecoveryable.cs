using System;
using UnityEngine;

namespace SHG
{
  using Character = TempCharacter;

  [Serializable]
  public struct Efficacy
  {
    [SerializeField]
    public Character.Stat Stat; 
    [SerializeField]
    public int Amount;
  }

  public interface IRecoveryable
  {
    public Efficacy[] Recovery();
  }
}

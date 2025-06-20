using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHG
{
  using Character = TempCharacter;

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

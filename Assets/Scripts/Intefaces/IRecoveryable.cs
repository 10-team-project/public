using System;
using UnityEngine;

//FIXME: 캐릭터 자원으로 연결해 주세요 
using Character = SHG.TempCharacter;

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

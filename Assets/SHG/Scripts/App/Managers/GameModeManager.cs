using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Patterns;

namespace SHG
{
  public interface IGameMode: IEquatable<IGameMode>
  {
    public IEnumerator OnStart();
    public IEnumerator OnEnd();
    public void OnStartFromEditor();
  }

  public class GameModeManager : MonoBehaviour
  {
    public bool IsSwitching { get; set; }
    public IGameMode CurrentMode { get; set; }
    WaitUntil WaitForSwitchable;

    void Awake()
    {
      this.IsSwitching = false;
      this.WaitForSwitchable = new WaitUntil(() => this.IsSwitching);
      #if UNITY_EDITOR
      var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
      switch (currentSceneIndex) {
        case 0:
          this.CurrentMode = MainMenuMode.Instance;
          this.CurrentMode.OnStartFromEditor();
          App.Instance.IsEditor = true;
            break;
        default:
          App.Instance.IsEditor = true;
          break;
      }
      #endif
    }

    IEnumerator SwitchMode(IGameMode gameMode)
    {
      yield return (this.WaitForSwitchable);
      if (gameMode == this.CurrentMode) {
        yield break;
      }
      this.IsSwitching = true;

      //yield return (App.Backdrop.Required(true));
      
      if (this.CurrentMode != null) {
        yield return (this.CurrentMode.OnEnd());
      }
      this.CurrentMode = gameMode; 
      //yield return (App.Backdrop.Release(true));
      this.IsSwitching = false;
    }
  }
}

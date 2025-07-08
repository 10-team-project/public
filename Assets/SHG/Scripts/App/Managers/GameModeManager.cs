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
    public string SceneName { get; }
    public IEnumerator OnStart();
    public IEnumerator OnEnd();
    public void OnStartFromEditor();
  }

  public class GameModeManager: SingletonBehaviour<GameModeManager>
  {
    const string SCENE_DIR = "Scenes";
    public Dictionary<string, GameScene> Scenes { get; private set; }
    public bool IsSwitching { get; set; }
    public IGameMode CurrentMode 
    { 
      get => this.currentMode;
      set {
        if (value != this.currentMode) {
          this.StartCoroutine(this.SwitchMode(value));
        }
      } 
    }
    WaitUntil WaitForSwitchable;
    IGameMode currentMode;

    protected override void Awake()
    {
      base.Awake();
      this.Scenes = new ();
      GameScene[] allScenes = Utils.LoadAllFrom<GameScene>(SCENE_DIR);
      foreach (var scene in allScenes) {
        this.Scenes[scene.Name] = scene;
      }
      this.IsSwitching = false;
      this.WaitForSwitchable = new WaitUntil(() => !this.IsSwitching);
      this.IsSwitching = false;
      #if !UNITY_EDITOR
      this.CurrentMode = new MainMenuMode();
      #endif
    }

    IEnumerator SwitchMode(IGameMode gameMode)
    {
      yield return (this.WaitForSwitchable);
      this.IsSwitching = true;
      if (this.CurrentMode != null) {
        yield return (this.CurrentMode.OnEnd());
      }
      this.currentMode = gameMode; 
      yield return (this.CurrentMode.OnStart());
      this.IsSwitching = false;
      if (App.Instance.IsEditor) {
        this.CurrentMode.OnStartFromEditor();
      }
    }
  }
}

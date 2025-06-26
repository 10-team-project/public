using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Patterns;
using KSH;
using LTH;

namespace SHG
{
  public enum GameMode
  {
    MainMenu,
    CharacterSelect,
    Farming,
    Shelter,
    Ending
  }

  public class App : SingletonBehaviour<App>
  {
    public bool IsEditor { get; private set; }
    public bool IsGamemodeControl => IsGamemodeControlEnabled;
    static bool IsGamemodeControlEnabled;
    GameModeManager gameModeManager;
    public IGameMode CurrentMode => this.gameModeManager.CurrentMode;
    List<ISingleton<MonoBehaviour>> managers;
    public TestSceneManager SceneManager { get; private set; }
    public InputManager InputManager { get; private set; }
    public Inventory Inventory { get; private set; }
    GameMode startMode = GameMode.MainMenu;
    
    [RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateApp()
    {
      var app = Instantiate(Resources.Load<GameObject>("App"));
      if (app == null) {
        throw new ApplicationException("Create App");
      }
      DontDestroyOnLoad(app);
      if (!IsGamemodeControlEnabled) {
        return ;
      }
    }

    protected override void Awake()
    {
      base.Awake();
      this.IsEditor = false;
      IsGamemodeControlEnabled = false;
      #if UNITY_EDITOR
      this.IsEditor = true;
      IsGamemodeControlEnabled = EditorPrefs.GetBool("IsGamemodeControlEnabled");
      this.managers = new ();
      this.SceneManager = TestSceneManager.CreateInstance();
      this.Inventory = Inventory.CreateInstance();
      this.InputManager = InputManager.CreateInstance();
      this.managers.Add(this.SceneManager as ISingleton<MonoBehaviour>);
      this.gameModeManager = GameModeManager.CreateInstance();
      foreach (var manager in this.managers) {
          if (manager is MonoBehaviour singletonBehaviour) {
            singletonBehaviour.transform.parent = this.transform;
          }
        }
      if (IsGamemodeControlEnabled) {
        this.ChangeMode(this.startMode);
      }
      #endif
    }

    public void ChangeMode(GameMode gameMode, string nextScene = null)
    {
      if (nextScene != null) {
        LoadingMode.Instance.SceneToLoad = nextScene;
        LoadingMode.Instance.OnLoaded = () => this.ChangeMode(gameMode);
        this.gameModeManager.CurrentMode = LoadingMode.Instance; 
      }
      else {
        var nextGameMode = this.selectGameMode(gameMode); 
        this.gameModeManager.CurrentMode = nextGameMode;
      }
    }

    IGameMode selectGameMode(GameMode gameMode)
    {
      switch (gameMode)
      {
        case GameMode.MainMenu:
          return (MainMenuMode.Instance);
        case GameMode.CharacterSelect:
          return (CharacterSelectMode.Instance);
        case GameMode.Farming:
          return (FarmingMode.Instance);
        case GameMode.Shelter:
          return (ShelterMode.Instance);
        default: 
          throw (new NotImplementedException());
      }
    }

    #if UNITY_EDITOR

    [MenuItem ("App/Gamemode control/enable")]
    static void Enable()
    {
      IsGamemodeControlEnabled = true;
      EditorPrefs.SetBool("IsGamemodeControlEnabled", true);
    }

    [MenuItem("App/Gamemode control/enable", true)]
    private static bool EnableValidate() {
      return (!IsGamemodeControlEnabled);
    }

    [MenuItem ("App/Gamemode control/disable")]
    static void Disable()
    {
      IsGamemodeControlEnabled = false;
      EditorPrefs.SetBool("IsGamemodeControlEnabled", false);
    }

    [MenuItem("App/Gamemode control/disable", true)]
    private static bool DisableValidate() {
      return (IsGamemodeControlEnabled);
    }
    #endif
  }
}

using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Patterns;
using KSH;
using LTH;
using NTJ;

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
    Component[] managers;
    public TestSceneManager SceneManager { get; private set; }
    public RecipeRegistry RecipeRegistry { get; private set; }
    public InputManager InputManager { get; private set; }
    public Inventory Inventory { get; private set; }
    public ItemLocker ItemStorage { get; private set; }
    public UIController UIController { get; private set; }
    public PopupManager PopupManager { get; private set; }
    public CameraController CameraController { get; private set; }
    public ItemTracker ItemTracker { get; private set; }
    public DropTable DropTable { get; private set; }
    public GameEventHandler GameEventHandler { get; private set; }
    public PlayerStatManager PlayerStatManager { get; private set; }
    public GameTimeManager GameTimeManager { get; private set; }
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
      IsGamemodeControlEnabled = true;
      this.SceneManager = TestSceneManager.CreateInstance();
      this.Inventory = new Inventory();
      this.ItemStorage = new ItemLocker();
      this.ItemTracker = new ItemTracker(this.Inventory);
      this.DropTable = new DropTable();
      this.DropTable.RegisterInventoryEvent(this.Inventory);
      this.InputManager = InputManager.CreateInstance();
      this.RecipeRegistry = RecipeRegistry.CreateInstance();
      this.UIController = UIController.CreateInstance();
      this.PlayerStatManager = PlayerStatManager.CreateInstance();
      this.GameTimeManager = new GameObject().AddComponent<GameTimeManager>();
      this.GameTimeManager.gameObject.SetActive(false);
      this.GameEventHandler = new GameEventHandler();
      this.GameEventHandler.RegisterItemTracker(this.ItemTracker);
      this.GameEventHandler.RegisterStatTracker(this.PlayerStatManager);
      //this.PopupManager = PopupManager.CreateInstance();
      this.PopupManager = Instantiate(Resources.Load<GameObject>("Popupmanager")).GetComponent<PopupManager>();
      this.managers = new Component[] {
        this.SceneManager,
        this.InputManager,
        this.RecipeRegistry,
        this.UIController,
        this.PopupManager,
        this.PlayerStatManager
      };
      this.gameModeManager = GameModeManager.CreateInstance();
      foreach (var manager in this.managers) {
        manager.transform.parent = this.transform;
      }
      #if UNITY_EDITOR
      this.IsEditor = true;
      IsGamemodeControlEnabled = EditorPrefs.GetBool("IsGamemodeControlEnabled");
      if (IsGamemodeControlEnabled) {
        this.ChangeMode(this.startMode);
      }
      #endif
    }

    public void SetCameraController(CameraController cameraController)
    {
      this.CameraController = cameraController;
    }

    public void ChangeMode(GameMode gameMode, string nextScene = null)
    {
      if (nextScene != null) {
        LoadingMode.Instance.SceneToLoad = nextScene;
        LoadingMode.Instance.OnLoaded = () => this.ChangeMode(gameMode);
        this.gameModeManager.CurrentMode = LoadingMode.Instance; 
      }
      else {
        var nextGameMode = this.SelectGameMode(gameMode); 
        this.gameModeManager.CurrentMode = nextGameMode;
      }
    }

    IGameMode SelectGameMode(GameMode gameMode)
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

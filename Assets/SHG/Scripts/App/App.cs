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
    public GameProgressManager GameProgressManager { get; private set; }
    public ScriptManager ScriptManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public GameObject CharacterPrefab => CharacterSelectMode.Instance.CharacterPrefab;
    public GameObject NpcPrefab => CharacterSelectMode.Instance.NpcPrefab;
    public EscapeConnector EscapeConnector { get; private set; }
    public ItemTrackerConnector ItemTrackerConnector { get; private set; }
    public EscapeManager EscapeManager { get; private set; }
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
      this.LoadItems();
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
      this.PlayerStatManager = Instantiate(Resources.Load<GameObject>("PlayerStatManager")).GetComponent<PlayerStatManager>();
      this.GameTimeManager = Instantiate(Resources.Load<GameObject>("GameTimeManager")).GetComponent<GameTimeManager>();
      this.GameTimeManager.gameObject.SetActive(false);
      this.GameEventHandler = new GameEventHandler();
      this.GameEventHandler.RegisterItemTracker(this.ItemTracker);
      this.GameEventHandler.RegisterStatTracker(this.PlayerStatManager);
      this.GameEventHandler.RegisterGameTimeTracker(this.GameTimeManager);
      this.Inventory.RegisterEventRewards(this.GameEventHandler);
      this.RecipeRegistry.RegisterItemUse(this.Inventory);
      this.ScriptManager = ScriptManager.Instance;
      this.GameProgressManager = GameProgressManager.CreateInstance();
      this.EscapeManager = EscapeManager.CreateInstance();
      this.EscapeConnector = new EscapeConnector(this.ItemTracker, this.Inventory);
      this.AudioManager = GameObject.Instantiate(
        Resources.Load<GameObject>("AudioManager")
        ).GetComponent<AudioManager>(); 
      this.ItemTrackerConnector = new ItemTrackerConnector();
      ItemTrackerConnector.ConnectToGameProgress(this.ItemTracker);
      this.PopupManager = Instantiate(Resources.Load<GameObject>("Popupmanager")).GetComponent<PopupManager>();

      this.managers = new Component[] {
        this.SceneManager,
        this.InputManager,
        this.RecipeRegistry,
        this.UIController,
        this.PopupManager,
        this.PlayerStatManager,
        this.GameTimeManager,
        this.GameProgressManager,
        this.EscapeManager,
        this.AudioManager
      };
      this.gameModeManager = GameModeManager.CreateInstance();
      foreach (var manager in this.managers) {
        manager.transform.parent = this.transform;
      }
      #if UNITY_EDITOR
      this.IsEditor = true;
      IsGamemodeControlEnabled = EditorPrefs.GetBool("IsGamemodeControlEnabled");
      #endif
      if (IsGamemodeControlEnabled) {
        this.ChangeMode(this.startMode);
      }
    }

    void LoadItems()
    {

      ItemStorageBase.LoadItems<DropChangeItemData>(ItemStorageBase.ITEM_DIRS[0]);
      ItemStorageBase.LoadItems<EquipmentItemData>(ItemStorageBase.ITEM_DIRS[1]);
      ItemStorageBase.LoadItems<PlainItemData>(ItemStorageBase.ITEM_DIRS[2]);
      ItemStorageBase.LoadItems<RecoveryItemData>(ItemStorageBase.ITEM_DIRS[3]);
      ItemStorageBase.LoadItems<StoryItemData>(ItemStorageBase.ITEM_DIRS[4]);
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

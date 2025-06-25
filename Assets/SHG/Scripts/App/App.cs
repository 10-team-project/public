using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Patterns;
using KSH;

namespace SHG
{
  public class App : SingletonBehaviour<App>
  {
    public bool IsEditor { get; set; }
    public static bool IsEnabled { get; private set;}
    GameModeManager gameModeManager;
    [RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateApp()
    {
      var app = Instantiate(Resources.Load<GameObject>("App"));
      if (app == null) {
        throw new ApplicationException("Create App");
      }
      Inventory.CreateInstance();
      DontDestroyOnLoad(app);
      if (!IsEnabled) {
        return ;
      }
    }

    protected override void Awake()
    {
      base.Awake();
      this.IsEditor = false;
      #if UNITY_EDITOR
      if (IsEnabled) {
        this.IsEditor = true;
        this.gameModeManager = new GameModeManager();
      }
      #endif
    }

    #if UNITY_EDITOR

    [MenuItem ("App/Gamemode control/enable")]
    static void Enable()
    {
      IsEnabled = true;
    }

    [MenuItem("App/Gamemode control/enable", true)]
    private static bool EnableValidate() {
      return (IsEnabled);
    }

    [MenuItem ("App/Gamemode control/disable")]
    static void Disable()
    {
      IsEnabled = false;
    }

    [MenuItem("App/Gamemode control/disable", true)]
    private static bool DisableValidate() {
      return (!IsEnabled);
    }
    #endif
  }
}

using System;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class App : SingletonBehaviour<App>
  {
    public bool IsEditor { get; set; }
    [RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateApp()
    {
      var app = Instantiate(Resources.Load<GameObject>("App"));
      if (app == null) {
        throw new ApplicationException("Create App");
      }
      Inventory.CreateInstance();
      DontDestroyOnLoad(app);
    }

    protected override void Awake()
    {
      base.Awake();
      this.IsEditor = false;
    }
  }
}

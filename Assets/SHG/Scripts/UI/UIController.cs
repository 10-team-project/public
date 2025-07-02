using System;
using System.Collections.Generic;
using LTH;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class UIController : SingletonBehaviour<UIController>, IInputLockHandler
  {
    public MainUIPlaceHolder MainUI { get; private set; }

    public void SetMainUI(MainUIPlaceHolder ui)
    {
      this.MainUI = ui;
    }

    public void OnInteractLocker()
    {
      if (this.MainUI != null && App.Instance?.InputManager != null &&
        !App.Instance.InputManager.IsBlocked(InputType.UI)) {
        this.MainUI.SetWindowVisible(MainUIPlaceHolder.WindowType.ItemLocker, true);
        this.MainUI.SetWindowVisible(MainUIPlaceHolder.WindowType.Inventory, true);
        App.Instance.InputManager.StartInput(this);
      }
      #if UNITY_EDITOR
      else {
        Debug.LogError("OnInteractLocker: Main ui is not set");
      }
      #endif
    }

    void Update()
    {
      if (this.MainUI != null) {
        if (Input.GetKeyDown(Settings.InputSettings.CloseWindowKey) &&
          !App.Instance.InputManager.IsBlocked(InputType.UI)) {
          this.MainUI.CloseAllWindows();
          App.Instance.InputManager.EndInput(this);
        }
        else if (
          Input.GetKeyDown(Settings.InputSettings.OpenInventoryKey) &&
          !App.Instance.InputManager.IsBlocked(InputType.UI)) {
          this.ToggleInventoryWindow();
        }
      }
    }

    void ToggleInventoryWindow()
    {
      if (this.MainUI.IsWindowOpened(MainUIPlaceHolder.WindowType.Inventory)) {

        this.MainUI.SetWindowVisible(MainUIPlaceHolder.WindowType.Inventory, false); 
        App.Instance.InputManager.EndInput(this);
      }
      else {
        this.MainUI.SetWindowVisible(MainUIPlaceHolder.WindowType.Inventory, true); 
        App.Instance.InputManager.StartInput(this);
      }
    }

    void OnDisable()
    {
      if (App.Instance?.InputManager  != null) {
        App.Instance.InputManager.EndInput(this);
      }
    }

    public bool IsInputBlocked(InputType inputType)
    {
      return (inputType == InputType.Interaction || inputType == InputType.Move);
    }

    public bool OnInputEnd()
    {
      if (this.MainUI != null) {
        this.MainUI.CloseAllWindows();
      }
      return (true);
    }

    public bool OnInputStart()
    {
      return (true);
    }
  }
}

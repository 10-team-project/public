using LTH;
using UnityEngine;
using Patterns;

namespace SHG
{
  public class UIController : SingletonBehaviour<UIController>, IInputLockHandler
  {

    public WindowUI MainUI { get; private set; }

    public void SetMainUI(WindowUI ui)
    {
      this.MainUI = ui;
    }

    public void Start()
    {
      App.Instance.GameTimeManager.OnSleep += () => {
        if (this.MainUI != null) {
          this.MainUI.CloseAllWindows();
        }
      };
    }

    public bool TryGetQuickSlotItem(int slotNumber, out EquipmentItemData item)
    {
      if (this.MainUI != null) {
        return (this.MainUI.TryGetQuickSlotItem(slotNumber, out item));
      }
      #if UNITY_EDITOR
      Debug.LogError("TryGetQuickSlotItem: Main UI is not set");
      #endif
      item = null;
      return (false);
    }

    public void ShowQuickSlot()
    {
      if (this.MainUI != null) {
        this.MainUI.SetWindowVisible(WindowUI.WindowType.QuickSlot, true);
      }
    }

    public void HideQuickSlot()
    {
      if (this.MainUI != null) {
        this.MainUI.SetWindowVisible(WindowUI.WindowType.QuickSlot, false);
      }
    }

    public void OnInteractCraft(CraftProvider provider)
    {
      CraftWindow.CurrentProvider = provider;
      this.OpenCraftWindow();
    }

    public void OpenCraftWindow()
    {
      App.Instance.AudioManager.PlaySFX(App.Instance.AudioManager.InventoryOpenSound);
      var mode = App.Instance.CurrentMode;
      if (mode is ShelterMode shelterMode) {
        CraftWindow.CurrentProvider = CraftProvider.NPC;
      }
      else {
        CraftWindow.CurrentProvider = CraftProvider.Table;
      }
      if (this.MainUI != null && App.Instance?.InputManager != null &&
        !App.Instance.InputManager.IsBlocked(InputType.UI)) {
        this.MainUI.SetWindowVisible(WindowUI.WindowType.Inventory, true); 
        this.MainUI.SetWindowVisible(WindowUI.WindowType.Craft, true); 
        App.Instance.InputManager.StartInput(this);
      }
      #if UNITY_EDITOR
      else {
        Debug.LogError("OnInteractLocker: Main ui is not set");
      }
      #endif
    }

    public void OnInteractLocker()
    {
      if (this.MainUI != null && App.Instance?.InputManager != null &&
        !App.Instance.InputManager.IsBlocked(InputType.UI)) {
        App.Instance.AudioManager.PlaySFX(App.Instance.AudioManager.InventoryOpenSound);
        this.MainUI.SetWindowVisible(WindowUI.WindowType.ItemLocker, true);
        this.MainUI.SetWindowVisible(WindowUI.WindowType.Inventory, true);
        this.MainUI.SetWindowVisible(WindowUI.WindowType.QuickSlot, false);
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
          this.MainUI.SetWindowVisible(WindowUI.WindowType.QuickSlot, true);
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
      if (this.MainUI.IsWindowOpened(WindowUI.WindowType.Inventory)) {

        this.CloseInventoryWindow();
      }
      else {
        this.OpenInventoryWindow();
      }
    }

    void OpenInventoryWindow()
    {
        App.Instance.AudioManager.PlaySFX(App.Instance.AudioManager.InventoryOpenSound);
      this.MainUI.SetWindowVisible(WindowUI.WindowType.Inventory, true); 
      App.Instance.InputManager.StartInput(this);
    }

    public void CloseInventoryWindow()
    {
      this.MainUI.SetWindowVisible(WindowUI.WindowType.Inventory, false); 
      if (this.MainUI.IsWindowOpened(
          WindowUI.WindowType.Craft)) {
        this.MainUI.SetWindowVisible(WindowUI.WindowType.Craft, false);
      }
      if (this.MainUI.IsWindowOpened(
          WindowUI.WindowType.ItemLocker
          )) {
        this.MainUI.SetWindowVisible(WindowUI.WindowType.ItemLocker, false);
      }
      App.Instance.InputManager.EndInput(this);
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

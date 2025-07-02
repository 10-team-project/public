using System;
using UnityEngine;

namespace SHG
{
  [Serializable]
  public class InputSettings
  {
    public KeyCode OpenInventoryKey;
    public KeyCode CloseWindowKey;
    KeyCode[] QuickSlotKeys;

    public InputSettings()
    {
      this.OpenInventoryKey = KeyCode.I;
      this.CloseWindowKey = KeyCode.Escape;
      this.QuickSlotKeys = new KeyCode[Inventory.QUICKSLOT_COUNT] {
        KeyCode.Alpha1, 
        KeyCode.Alpha2, 
        KeyCode.Alpha3, 
        KeyCode.Alpha4, 
      };
    }
  }

  [Serializable]
  public static class Settings 
  {
    public static InputSettings InputSettings;

    static Settings()
    {
      InputSettings = new InputSettings();
    }
  }
}

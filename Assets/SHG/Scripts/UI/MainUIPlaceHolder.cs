using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using EditorAttributes;
using Patterns;
using KSH;

namespace SHG
{

  public class MainUIPlaceHolder : MonoBehaviour
  {
    [Flags]
    public enum WindowType
    {
      None = 0,
      Inventory = 1,
      ItemLocker = 2,
      Craft = 4
    }
    public static readonly WindowType[] ALL_WINDOW_TYPES = (WindowType[])Enum.GetValues(typeof(WindowType));

    int currentOpenedWindows = (int)WindowType.None;
    VisualElement root;
    InventoryContainerWindow inventoryWindow;
    QuickSlotWindow quickSlotWindow;
    CraftWindow craftWindow;
    ItemBox floatingItemBox;
    ItemLockerContainerWindow itemLockerWindow;

    ItemSpawnTest itemSpawner;

    Button inventoryButton;
    Button spawnItemButton;
    Button toggleGameTimeButton;
    Button storageButton;
    Button craftButton;
    GameObject gameTimeUI;

    Label hpLabel;
    Label thirstLabel;
    Label hungerLabel;
    Label fatigueLabel;
    Resource hp;
    Resource thirst;
    Resource hunger;
    Resource fatigue;

    public WindowType[] GetCurrentOpenedWindowTypes()
    {
      var count = (Utils.CountBits(this.currentOpenedWindows));
      int index = 0;
      var windowTypes = new WindowType[count];
      foreach (var windowType in ALL_WINDOW_TYPES) {
        if (this.IsWindowOpened(windowType))  {
          windowTypes[index++] = windowType;
        }
      }
      return (windowTypes); 
    }

    public bool IsWindowOpened(WindowType windowType)
    {
      return ((this.currentOpenedWindows & ((int)windowType)) != 0);
    }

    public void CloseAllWindows()
    {
      foreach (var windowType in ALL_WINDOW_TYPES) {
        if (this.IsWindowOpened(windowType)) {
          this.SetWindowVisible(windowType, false);
        }
      }
    }

    void Awake()
    {
      this.root = this.GetComponent<UIDocument>().rootVisualElement;
      this.root.style.width = Length.Percent(100);
      this.root.style.height = Length.Percent(100);
      var buttonContainer = new VisualElement();
      buttonContainer.style.position = Position.Absolute;
      buttonContainer.style.left = Length.Percent(5);
      buttonContainer.style.top= Length.Percent(5);
      this.inventoryButton = new Button();
      this.inventoryButton.text = "Inventory";
      this.inventoryButton.AddToClassList("test-button");
      buttonContainer.Add(this.inventoryButton);
      this.spawnItemButton = new Button();
      this.spawnItemButton.text = "SpawnItem";
      this.spawnItemButton.AddToClassList("test-button");
      buttonContainer.Add(this.spawnItemButton);
      this.toggleGameTimeButton = new Button();
      this.toggleGameTimeButton.text = "Game Time";
      this.toggleGameTimeButton.AddToClassList("test-button");
      buttonContainer.Add(this.toggleGameTimeButton);
      this.craftButton = new Button();
      this.craftButton.text = "Craft";
      this.craftButton.AddToClassList("test-button");
      buttonContainer.Add(this.craftButton);

      this.storageButton = new Button();
      this.storageButton.text = "Storage";
      this.storageButton.AddToClassList("test-button");
      buttonContainer.Add(this.storageButton);

      var statContainer = new VisualElement();
      statContainer.style.position = Position.Absolute;
      statContainer.style.top = Length.Percent(5);
      statContainer.style.right = Length.Percent(10);
      
      this.hpLabel = new Label();
      this.thirstLabel = new Label();
      this.fatigueLabel = new Label();
      this.hungerLabel = new Label();
      statContainer.Add(this.hpLabel);
      statContainer.Add(this.thirstLabel);
      statContainer.Add(this.fatigueLabel);
      statContainer.Add(this.hungerLabel);
      statContainer.name = "stat-container";
      this.root.Add(statContainer);
      this.root.Add(buttonContainer);

      this.inventoryButton.RegisterCallback<ClickEvent>(this.OnClickInventoryButton);
      this.craftButton.RegisterCallback<ClickEvent>(this.OnClickCraftButton);
      this.storageButton.RegisterCallback<ClickEvent>(this.OnClickStorageButton);
      this.toggleGameTimeButton.RegisterCallback<ClickEvent>(click => 
        this.gameTimeUI.SetActive(!this.gameTimeUI.activeSelf)
        );
      this.spawnItemButton.RegisterCallback<ClickEvent>(click => 
        this.itemSpawner.SpawnItem(1)
        );
      this.CreateItemUI();
    }

    void OnEnable()
    {
      App.Instance?.UIController.SetMainUI(this); 
    }

    void OnDisable()
    {
      App.Instance?.UIController.SetMainUI(null); 
    }

    public void SetWindowVisible(WindowType windowType, bool visible)
    {
      IHideableUI window = windowType switch {
        WindowType.Inventory => this.inventoryWindow,
        WindowType.ItemLocker => this.itemLockerWindow,
        WindowType.Craft => this.craftWindow,
        _ => throw (new ArgumentException())
      };
      this.SetWindowVisible(window, windowType, visible);
    }

    public void ToggleWindowVisible(WindowType windowType)
    {
      this.SetWindowVisible(
        windowType, 
        this.IsWindowOpened(windowType) ? false: true);
    }

    void SetWindowVisible(IHideableUI window, bool visible)
    {
      WindowType windowType = window switch {
        InventoryContainerWindow inventory => WindowType.Inventory,
        ItemLockerContainerWindow itemLocker => WindowType.ItemLocker,
        CraftWindow craftWindow => WindowType.Craft,
        _ => throw (new NotImplementedException($"unknown window type for {window}"))
      };
      this.SetWindowVisible(window, windowType, visible);
    }

    void SetWindowVisible(IHideableUI window, WindowType windowType, bool visible)
    {
      if (visible && !window.IsVisiable) {
        window.Show();
        this.currentOpenedWindows |= ((int)windowType);
      }
      else if (!visible && window.IsVisiable) {
        window.Hide();
        this.currentOpenedWindows &= (~((int)windowType));
      }
    }

    void OnClickInventoryButton(ClickEvent click)
    {
      if (inventoryWindow.IsVisiable) {
        this.SetWindowVisible(this.inventoryWindow, false);
      }
      else {
        this.SetWindowVisible(this.inventoryWindow, true);
      }
    }

    void OnClickCraftButton(ClickEvent click)
    {
      if (craftWindow.IsVisiable) {
        this.SetWindowVisible(this.craftWindow, false); 
        this.SetWindowVisible(this.inventoryWindow, false);
      }
      else {
        this.SetWindowVisible(this.craftWindow, true); 
        this.SetWindowVisible(this.inventoryWindow, true);
      }
    }

    void OnClickStorageButton(ClickEvent click)
    {
      if (this.itemLockerWindow.IsVisiable) {
        this.SetWindowVisible(this.itemLockerWindow, false);
        this.SetWindowVisible(this.inventoryWindow, false);
      }
      else {
        this.SetWindowVisible(this.itemLockerWindow, true);
        this.SetWindowVisible(this.inventoryWindow, true);
      }
    }

    void CreateItemUI()
    {
      this.floatingItemBox = this.CreateFloatingItemBox();
      this.inventoryWindow = new InventoryContainerWindow(this.floatingItemBox);
      this.quickSlotWindow = new QuickSlotWindow(this.floatingItemBox);
      this.craftWindow = new CraftWindow(this.floatingItemBox);
      this.itemLockerWindow = new ItemLockerContainerWindow(this.floatingItemBox);
      this.inventoryWindow.Hide();
      this.craftWindow.Hide();
      this.itemLockerWindow.Hide();
      this.root.Add(this.itemLockerWindow);
      this.root.Add(this.inventoryWindow);
      this.root.Add(this.quickSlotWindow);
      this.root.Add(this.floatingItemBox);
      this.root.Add(this.craftWindow);
      this.WireInventoryStoarges();
    }

    void WireInventoryStoarges()
    {
      this.inventoryWindow.AddDropTargets(
        new ItemStorageWindow[] { 
        this.quickSlotWindow,
        this.itemLockerWindow.ItemContainer
        }
        );
      this.itemLockerWindow.ItemContainer.AddDropTargets(
        new ItemStorageWindow[] {
          this.inventoryWindow.NormalItemTab.Content as InventoryWindow,
          this.inventoryWindow.StoryItemTab.Content as InventoryWindow
        }
        );
      this.quickSlotWindow.AddDropTargets(
        new ItemStorageWindow[] { 
        this.inventoryWindow.NormalItemTab.Content as ItemLockerWindow
        }
        );
    }

    void Start()
    {
      this.itemSpawner = GameObject.Find("ItemSpawn")?.GetComponent<ItemSpawnTest>();
      this.gameTimeUI = GameObject.Find("GameTimeUI");
      if (this.gameTimeUI != null) {
        this.gameTimeUI.SetActive(false);
      }
      if (GameObject.Find("HPResource") == null) {
        return ;
      }
      this.hp = GameObject.Find("HPResource").GetComponent<Resource>();
      this.thirst = GameObject.Find("ThirstyResource").GetComponent<Resource>();
      this.fatigue = GameObject.Find("FatigueResource").GetComponent<Resource>();
      this.hunger = GameObject.Find("HungerResource").GetComponent<Resource>();

    }

    ItemBox CreateFloatingItemBox()
    {
      var floatingItemBox = new ItemBox(this.root);
      floatingItemBox.AddToClassList("item-box-floating");
      floatingItemBox.Hide();
      return (floatingItemBox);
    }

    void Update()
    {
      if (this.hpLabel == null || this.hp == null) {
        return ;
      }
      this.hpLabel.text = $"HP: {this.hp.Cur}/{this.hp.Max}";
      this.hungerLabel.text = $"Hunger: {this.hunger.Cur}/{this.hunger.Max}";
      this.thirstLabel.text = $"Thirst : {this.thirst.Cur}/{this.thirst.Max}";
      this.fatigueLabel.text = $"Fatigue: {this.fatigue.Cur}/{this.fatigue.Max}";
    }

    [Button ("Show inventory")]
    void ShowInventory()
    {
      this.inventoryWindow.Show();   
    }
    
    [Button ("Hide inventory")]
    void HideInventory()
    {
      this.inventoryWindow.Hide();
    }
  }
}

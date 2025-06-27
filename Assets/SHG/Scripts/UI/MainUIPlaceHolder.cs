using UnityEngine;
using UnityEngine.UIElements;
using EditorAttributes;
using Patterns;
using KSH;
using System;

namespace SHG
{
  public class MainUIPlaceHolder : MonoBehaviour
  {
    VisualElement root;
    InventoryWindow inventoryWindow;
    QuickSlotWindow quickSlotWindow;
    CraftWindow craftWindow;
    ItemBox floatingItemBox;
    ItemSpawnTest itemSpawner;

    Button inventoryButton;
    Button spawnItemButton;
    Button toggleGameTimeButton;
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
      this.toggleGameTimeButton.RegisterCallback<ClickEvent>(click => {
        this.gameTimeUI.SetActive(!this.gameTimeUI.activeSelf);
        });
      this.spawnItemButton.RegisterCallback<ClickEvent>(click => {
        this.itemSpawner.SpawnItem(1);
        });
      this.CreateItemUI();
    }

    void OnClickCraftButton(ClickEvent click)
    {
      if (craftWindow.IsVisiable) {
        this.craftWindow.Hide();
      }
      else {
        this.craftWindow.Show(); 
      }
    }

    void CreateItemUI()
    {
      this.floatingItemBox = this.CreateFloatingItemBox();
      this.inventoryWindow = new InventoryWindow(this.floatingItemBox);
      this.quickSlotWindow = new QuickSlotWindow(this.floatingItemBox);
      this.craftWindow = new CraftWindow(this.floatingItemBox);
      this.inventoryWindow.Hide();
      this.craftWindow.Hide();
      this.root.Add(this.inventoryWindow);
      this.root.Add(this.quickSlotWindow);
      this.root.Add(this.floatingItemBox);
      this.root.Add(this.craftWindow);
      this.WireInventoryStoarges();
    }

    void WireInventoryStoarges()
    {
      this.inventoryWindow.SetDropTargets(new ItemStorageWindow[1] {
        this.quickSlotWindow
        });
      this.quickSlotWindow.SetDropTargets(new ItemStorageWindow[1] {
        this.inventoryWindow
        });
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

    void OnClickInventoryButton(ClickEvent click)
    {
      if (inventoryWindow.IsVisiable) {
        inventoryWindow.Hide();
      }
      else {
        inventoryWindow.Show();
      }
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

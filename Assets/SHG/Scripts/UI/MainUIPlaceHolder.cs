using UnityEngine;
using UnityEngine.UIElements;
using EditorAttributes;
using Patterns;
using KSH;

namespace SHG
{
  public class MainUIPlaceHolder : MonoBehaviour
  {
    VisualElement root;
    InventoryWindow inventoryWindow;
    QuickSlotWindow quickSlotWindow;
    Button inventoryButton;
    ObservableValue<(ItemAndCount, VisualElement)> currentDragging;
    ItemBox floatingItemBox;
    ItemSpawnTest itemSpawner;
    Button spawnItemButton;
    Button toggleGameTimeButton;
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
      this.currentDragging = new ((ItemAndCount.None, null));
      this.root = this.GetComponent<UIDocument>().rootVisualElement;
      this.root.style.width = Length.Percent(100);
      this.root.style.height = Length.Percent(100);
      this.floatingItemBox = this.CreateFloatingItemBox();
      this.inventoryWindow = new InventoryWindow(this.floatingItemBox);
      this.inventoryWindow.Hide();
      this.root.Add(this.inventoryWindow);
      var buttonContainer = new VisualElement();
      buttonContainer.style.position = Position.Absolute;
      buttonContainer.style.left = Length.Percent(5);
      buttonContainer.style.top= Length.Percent(5);
      this.inventoryButton = new Button();
      this.inventoryButton.text = "Inventory";
      this.inventoryButton.AddToClassList("test-button");
      buttonContainer.Add(this.inventoryButton);
      this.quickSlotWindow = new QuickSlotWindow(this.floatingItemBox);
      this.spawnItemButton = new Button();
      this.spawnItemButton.text = "SpawnItem";
      this.spawnItemButton.AddToClassList("test-button");
      buttonContainer.Add(this.spawnItemButton);
      this.toggleGameTimeButton = new Button();
      this.toggleGameTimeButton.text = "Game Time";
      this.toggleGameTimeButton.AddToClassList("test-button");
      buttonContainer.Add(this.toggleGameTimeButton);

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
      this.root.Add(this.quickSlotWindow);
      this.root.Add(this.floatingItemBox);
      this.root.Add(buttonContainer);

      this.inventoryButton.RegisterCallback<ClickEvent>(this.OnClickInventoryButton);
      this.toggleGameTimeButton.RegisterCallback<ClickEvent>(click => {
        this.gameTimeUI.SetActive(!this.gameTimeUI.activeSelf);
        });
      this.spawnItemButton.RegisterCallback<ClickEvent>(click => {
        this.itemSpawner.SpawnItem(1);
        });
    }

    void Start()
    {
      this.itemSpawner = GameObject.Find("ItemSpawn").GetComponent<ItemSpawnTest>();
      this.gameTimeUI = GameObject.Find("GameTimeUI");
      this.gameTimeUI.SetActive(false);
      this.hp = GameObject.Find("HPResource").GetComponent<Resource>();
      this.thirst = GameObject.Find("ThirstyResource").GetComponent<Resource>();
      this.fatigue = GameObject.Find("FatigueResource").GetComponent<Resource>();
      this.hunger = GameObject.Find("HungerResource").GetComponent<Resource>();

    }

    ItemBox CreateFloatingItemBox()
    {
      var floatingItemBox = new ItemBox(this.root);
      floatingItemBox.AddToClassList("inventory-floating-itembox");
      floatingItemBox.Hide();
      return (floatingItemBox);
    }

    void Update()
    {
      this.hpLabel.text = $"HP: {this.hp.Cur}/{this.hp.Max}";
      this.hungerLabel.text = $"Hunger: {this.hunger.Cur}/{this.hunger.Max}";
      this.thirstLabel.text = $"Thirst : {this.thirst.Cur}/{this.thirst.Max}";
      this.fatigueLabel.text = $"Fatigue: {this.fatigue.Cur}/{this.fatigue.Max}";
    }

    void OnEnable()
    {
      this.quickSlotWindow.RegisterCallback<PointerUpEvent>(this.OnPointerUp);
      this.inventoryWindow.RegisterCallback<PointerUpEvent>(this.OnPointerUp);
    }

    void OnDisable()
    {
    }

    void OnStartDragItem(ItemAndCount itemAndCount, VisualElement window)
    {
      this.currentDragging.Value = (itemAndCount, window);
    }

    void OnEndDragItem(ItemAndCount itemAndCount, VisualElement window)
    {

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

    void OnPointerUp(PointerUpEvent pointerUpEvent)
    {
    
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

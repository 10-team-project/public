using UnityEngine;
using UnityEngine.UIElements;
using EditorAttributes;
using Patterns;

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
      this.inventoryButton = new Button();
      this.inventoryButton.text = "Inventory";
      this.inventoryButton.style.position = Position.Absolute;
      this.inventoryButton.style.width = new Length(200);
      this.inventoryButton.style.height = new Length(100);
      this.inventoryButton.style.top = Length.Percent(5);
      this.inventoryButton.style.right = Length.Percent(5);
      this.inventoryButton.RegisterCallback<ClickEvent>(this.OnClickInventoryButton);
      this.quickSlotWindow = new QuickSlotWindow(this.floatingItemBox);
      this.root.Add(this.quickSlotWindow);
      this.root.Add(this.inventoryButton);
      this.root.Add(this.floatingItemBox);
    }

    ItemBox CreateFloatingItemBox()
    {
      var floatingItemBox = new ItemBox(this.root);
      floatingItemBox.AddToClassList("inventory-floating-itembox");
      floatingItemBox.Hide();
      return (floatingItemBox);
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
      Debug.Log($"PointerUp "); 
    
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

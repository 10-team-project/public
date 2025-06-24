using UnityEngine;
using UnityEngine.UIElements;
using EditorAttributes;

namespace SHG
{
  public class MainUIPlaceHolder : MonoBehaviour
  {
    VisualElement root;
    InventoryWindow inventoryWindow;
    Button inventoryButton;

    void Awake()
    {
      this.root = this.GetComponent<UIDocument>().rootVisualElement;
      this.root.style.width = Length.Percent(100);
      this.root.style.height = Length.Percent(100);
      this.inventoryWindow = new InventoryWindow();
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
      this.root.Add(this.inventoryButton);
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

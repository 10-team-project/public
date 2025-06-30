using UnityEngine;

namespace SHG
{
  public class ItemCraftInteract : MonoBehaviour, IInteractable
  {
    public void Interact()
    {
      App.Instance.UIController.OnInteractCraft();
    }
  }
}

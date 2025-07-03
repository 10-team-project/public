using UnityEngine;

namespace SHG
{
  public class ItemLockerInteract : MonoBehaviour, IInteractable
  {
    public void Interact()
    {
      App.Instance.UIController.OnInteractLocker();
    }
  }
}

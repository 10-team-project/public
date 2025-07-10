using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class ItemCraftInteract : MonoBehaviour, IInteractable
  {
    [SerializeField] [Validate ("Provider can not be all", nameof(IsProviderAll), MessageMode.Error)]
    CraftProvider provider;

    protected bool IsProviderAll() => (this.provider == CraftProvider.All);

    public void Interact()
    {
      App.Instance.UIController.OnInteractCraft(this.provider);
    }
  }
}

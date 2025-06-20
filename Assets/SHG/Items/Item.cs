using UnityEngine;
namespace SHG
{
  public abstract class Item 
  {
    public ItemData Data => this.data; 
    [SerializeField]
    ItemData data;

    public Item(ItemData data)
    {
      this.data = data;
    }
  }
}

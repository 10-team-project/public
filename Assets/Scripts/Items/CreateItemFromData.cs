using System;
using UnityEngine;
using SHG;

public partial class Item
{

  static GameObject LOOT_PREFAB;

  static Item()
  {
    LOOT_PREFAB = Resources.Load<GameObject>("ItemLootPrefab"); 
  }

  static readonly int ITEM_LAYER = LayerMask.NameToLayer("Item");

  public static Item CreateItemFrom(ItemData itemData)
  {
    Item item;
    switch (itemData)
    {
      case RecoveryItemData recoveryItemData:
        item = new RecoveryItem(recoveryItemData); 
        break;
      case PlainItemData plainItemData:
        item = new PlainItem(itemData);
        break;
      case EquipmentItemData equipmentItemData:
        item = new EquipmentItem(equipmentItemData);
        break;
      case DropChangeItemData dropChangeItemData:
        item = new DropChangeItem(dropChangeItemData);
        break;
      case StoryItemData storyItemData:
        item = new StoryItem(storyItemData);
        break;
      default:
      throw (new NotImplementedException());
    }
    return (item);
  }

  public static ItemObject CreateItemObjectFrom(Item item)
  {
    var gameObject = GameObject.Instantiate(LOOT_PREFAB);
    gameObject.name = item.Data.Name;
    var itemObject = gameObject.GetComponent<ItemObject>();
    itemObject.OnPickedUp += App.Instance.Inventory.AddItem;
    itemObject.SetItem(item);
    return (itemObject);
  }

  public static ItemObject CreateItemObjectFrom(ItemData itemData)
  {
    var item = Item.CreateItemFrom(itemData);
    Debug.Log(item);
    return (Item.CreateItemObjectFrom(item));
  }

}

using System;
using UnityEngine;
using SHG;

public partial class Item
{

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
    var gameObject = GameObject.Instantiate(item.Data.Prefab);
    gameObject.name = item.Data.Name;
    gameObject.layer = ITEM_LAYER;
    //FIXME: 테스트 끝나면 직접 설정하기
    #if UNITY_EDITOR
    if (gameObject.GetComponent<Collider>() == null) {
      var sphereCollider = gameObject.AddComponent<SphereCollider>();
      sphereCollider.radius = 1f;
      sphereCollider.isTrigger = true;
    }
    #endif
    var itemObject = gameObject.AddComponent<ItemObject>();
    itemObject.OnPickedUp += Inventory.Instance.AddItem;
    itemObject.SetItem(item);
    return (itemObject);
  }

  public static ItemObject CreateItemObjectFrom(ItemData itemData)
  {
    if (itemData.Prefab == null) {
      throw (new ArgumentException($"{itemData.Name} has no Prefab"));
    }
    var item = Item.CreateItemFrom(itemData);
    return (Item.CreateItemObjectFrom(item));
  }

}

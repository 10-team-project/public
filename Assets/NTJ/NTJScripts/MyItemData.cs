using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/New Item")]
public class MyItemData : ItemData
{
    [SerializeField] private string id;
    public string ID => id;
}

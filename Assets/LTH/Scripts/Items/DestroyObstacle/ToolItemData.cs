using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LTH
{
    // ToDo 파괴 아이템 구분 필요
    public enum ToolType
    {
        None,
        Hammer,
        Lockfit,
        Pipe,
        Spanner
    }

    [CreateAssetMenu(menuName = "Item/ToolItem")]
    public class ToolItemData : MonoBehaviour
    {
        public ToolType toolType;
    }
}

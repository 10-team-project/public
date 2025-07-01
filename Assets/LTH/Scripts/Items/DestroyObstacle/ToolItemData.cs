using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LTH
{
    // ToDo �ı� ������ ���� �ʿ�
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

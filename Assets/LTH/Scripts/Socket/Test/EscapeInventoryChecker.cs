using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SHG;
using UnityEngine;

namespace LTH
{
    public class EscapeInventoryChecker : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                CheckEscapeItemsInInventory();
            }
        }

        void CheckEscapeItemsInInventory()
        {
            var inventory = App.Instance.Inventory;
            var requiredItems = EscapeManager.Instance.GetRequiredEscapeItems();

            bool allPresent = requiredItems.All(item =>
                inventory.GetItemCount(item) > 0
            );

            Debug.Log(allPresent
                ? "인벤토리에 모든 탈출 아이템이 있음! 탈출 성공 가능!"
                : "아직 부족한 아이템이 있음!");
        }
    }
}

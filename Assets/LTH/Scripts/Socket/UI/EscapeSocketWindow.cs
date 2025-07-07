using System.Collections;
using System.Collections.Generic;
using SHG;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Ż�� ���� ���� UI
/// </summary>
/// <remarks>
/// �� Ŭ������ Ż�� ������ ���� UI�� �����մϴ�.
/// ItemStorageWindow�� ����Ͽ� �����Ǹ�, ���Ͽ� �������� �巡�� �� ����Ͽ� ����� �� �ֽ��ϴ�.
/// ������ 3ĭ ����
/// �巡�� �� ������� �κ��丮���� �������� �ű�
/// EscapeSocketManager�� ������ ����
/// UI�� �ش� ������ ���� ǥ��
/// ��õ��� �� �ֵ��� ���� �ʱ�ȭ ����
/// </summary>

namespace LTH
{
    public class EscapeSocketWindow : ItemStorageWindow
    {
        const int SOCKET_COUNT = 3;
        protected override Vector2 DescriptionOffset => new Vector2(50f, 100f);
        private List<ItemBox> socketBoxes = new();

        public EscapeSocketWindow(ItemBox floatingItemBox, VisualElement floatingDescriptionContainer)
            : base(floatingItemBox, null) // ������ �κ��丮 ����� �ƴ�
        {
            this.itemDescriptionContainer = floatingDescriptionContainer;
            this.itemDescriptionTitle = floatingDescriptionContainer.Q(className: "item-storage-item-description-title") as Label;
            this.itemDescriptionContent = floatingDescriptionContainer.Q(className: "item-storage-item-description-content") as Label;
        }

        /// <summary>
        /// 3�� ���� ���� �� ItemBox 3�� ����
        /// </summary>
        protected override void CreateUI()
        {
            for (int i = 0; i < SOCKET_COUNT; i++)
            {
                var box = new ItemBox(this);
                box.SetLabel($"���� {i + 1}");
                box.RegisterCallback<PointerDownEvent>(this.OnPointerDown);
                box.RegisterCallback<PointerUpEvent>(this.OnPointerUp);
                box.RegisterCallback<PointerMoveEvent>(this.OnPointerMove);
                box.RegisterCallback<PointerOverEvent>(this.OnPointerOver);
                box.RegisterCallback<PointerLeaveEvent>(this.OnPointerLeave);

                this.itemsContainer.Add(box);
                this.socketBoxes.Add(box);
            }
        }

        protected override void FillItems(ItemStorageBase inventory)
        {
            // ������ �ܺ� �κ��丮 ������� ä���� ����
        }

        protected override void OnUsePointerButtonDown(ItemBox boxElement, ItemAndCount itemAndCount)
        {
            // ���Ͽ����� ������ ��� �Ұ�
        }

        protected override bool IsAbleToDropItem(ItemData item, ItemStorageWindow targetContainer)
        {
            // �κ��丮 �� ���� ����� ���
            return true;
        }


        /// <summary>
        /// EscapeSocketManager.TryInsertItem() �� �����ϸ� UI�� ǥ��
        /// </summary>
        protected override void DropItem(ItemAndCount itemAndCount, ItemStorageWindow targetContainer)
        {
            // EscapeSocketManager�� ��� �õ�
            bool success = EscapeSocketManager.Instance.TryInsertItem(itemAndCount.Item);
            if (!success) return;

            // ��� �ִ� ���� �ڽ��� ������ ǥ��
            foreach (var box in this.socketBoxes)
            {
                if (box.ItemData == ItemAndCount.None)
                {
                    box.SetData(new ItemAndCount { Item = itemAndCount.Item, Count = 1 });
                    break;
                }
            }
        }

        protected override bool IsAbleToDropOutSide(ItemData item)
        {
            // ���Ͽ����� ������ ���� �� ����
            return false;
        }

        protected override void DropItemOutSide(ItemAndCount itemAndCount)
        {
            // �ƹ��͵� ���� ����
        }

       
        protected override void ClearItems()
        {
            foreach (var box in this.socketBoxes)
            {
                box.RemoveData();
            }
            EscapeSocketManager.Instance.ClearSockets();
        }

        /// <summary>
        /// ����
        /// </summary>
        public void ResetSocketUI()
        {
            this.ClearItems();
        }

        public bool IsFilled()
        {
            return this.socketBoxes.TrueForAll(box => box.ItemData != ItemAndCount.None);
        }

        /// �׽�Ʈ �� �ܺ� ȣ��� Ż�� ���� ����
        public bool CheckEscapeCondition()
        {
            if (!IsFilled())
            {
                Debug.LogWarning("������ ���� �� ���� �ʾҽ��ϴ�.");
                return false;
            }

            bool result = EscapeSocketManager.Instance.CheckEscapeSuccess();
            Debug.Log(result ? "Ż�� ���� ����" : "Ż�� ���� ������");
            return result;
        }
    }
}

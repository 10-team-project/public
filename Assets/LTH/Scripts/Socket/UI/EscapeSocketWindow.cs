using System.Collections;
using System.Collections.Generic;
using SHG;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 탈출 소켓 슬롯 UI
/// </summary>
/// <remarks>
/// 이 클래스는 탈출 소켓의 슬롯 UI를 관리합니다.
/// ItemStorageWindow를 상속하여 구현되며, 소켓에 아이템을 드래그 앤 드랍하여 등록할 수 있습니다.
/// 소켓은 3칸 고정
/// 드래그 앤 드랍으로 인벤토리에서 아이템을 옮김
/// EscapeSocketManager에 아이템 전달
/// UI에 해당 아이템 정보 표시
/// 재시도할 수 있도록 슬롯 초기화 가능
/// </summary>

namespace LTH
{
    public class EscapeSocketWindow : ItemStorageWindow
    {
        const int SOCKET_COUNT = 3;
        protected override Vector2 DescriptionOffset => new Vector2(50f, 100f);
        private List<ItemBox> socketBoxes = new();

        public EscapeSocketWindow(ItemBox floatingItemBox, VisualElement floatingDescriptionContainer)
            : base(floatingItemBox, null) // 소켓은 인벤토리 기반이 아님
        {
            this.itemDescriptionContainer = floatingDescriptionContainer;
            this.itemDescriptionTitle = floatingDescriptionContainer.Q(className: "item-storage-item-description-title") as Label;
            this.itemDescriptionContent = floatingDescriptionContainer.Q(className: "item-storage-item-description-content") as Label;
        }

        /// <summary>
        /// 3개 소켓 고정 → ItemBox 3개 생성
        /// </summary>
        protected override void CreateUI()
        {
            for (int i = 0; i < SOCKET_COUNT; i++)
            {
                var box = new ItemBox(this);
                box.SetLabel($"소켓 {i + 1}");
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
            // 소켓은 외부 인벤토리 기반으로 채우지 않음
        }

        protected override void OnUsePointerButtonDown(ItemBox boxElement, ItemAndCount itemAndCount)
        {
            // 소켓에서는 아이템 사용 불가
        }

        protected override bool IsAbleToDropItem(ItemData item, ItemStorageWindow targetContainer)
        {
            // 인벤토리 → 소켓 드랍만 허용
            return true;
        }


        /// <summary>
        /// EscapeSocketManager.TryInsertItem() → 성공하면 UI에 표시
        /// </summary>
        protected override void DropItem(ItemAndCount itemAndCount, ItemStorageWindow targetContainer)
        {
            // EscapeSocketManager에 등록 시도
            bool success = EscapeSocketManager.Instance.TryInsertItem(itemAndCount.Item);
            if (!success) return;

            // 비어 있는 소켓 박스에 아이템 표시
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
            // 소켓에서는 아이템 버릴 수 없음
            return false;
        }

        protected override void DropItemOutSide(ItemAndCount itemAndCount)
        {
            // 아무것도 하지 않음
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
        /// 리셋
        /// </summary>
        public void ResetSocketUI()
        {
            this.ClearItems();
        }

        public bool IsFilled()
        {
            return this.socketBoxes.TrueForAll(box => box.ItemData != ItemAndCount.None);
        }

        /// 테스트 및 외부 호출용 탈출 조건 판정
        public bool CheckEscapeCondition()
        {
            if (!IsFilled())
            {
                Debug.LogWarning("소켓이 아직 다 차지 않았습니다.");
                return false;
            }

            bool result = EscapeSocketManager.Instance.CheckEscapeSuccess();
            Debug.Log(result ? "탈출 조건 충족" : "탈출 조건 불충족");
            return result;
        }
    }
}

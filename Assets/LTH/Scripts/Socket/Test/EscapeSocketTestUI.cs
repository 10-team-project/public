using System.Collections;
using System.Collections.Generic;
using SHG;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 탈출 소켓 테스트 UI 클래스
/// </summary>
/// <remarks>
/// 이 클래스는 탈출 소켓 테스트를 위한 UI를 구현합니다.
/// 1. EscapeSocketWindow를 UI 상에 띄우기
/// 2. InventoryWindow를 생성하여 아이템을 드래그 앤 드롭할 수 있도록 설정
/// 3. FloatingItemBox와 FloatingDescription을 사용하여 아이템 정보를 표시
/// 4. 테스트 버튼을 추가하여 탈출 성공 여부를 확인할 수 있도록 함
/// 5. EscapeSocketManager를 사용하여 탈출 조건을 확인
/// 6. 인벤토리에서 아이템을 드래그 앤 드롭하여 탈출 소켓에 삽입할 수 있도록 설정
/// 7. EscapeSocketManager의 CheckEscapeSuccess 메서드를 호출하여 탈출 조건이 충족되었는지 확인
/// 8. EscapeSocketWindow와 InventoryWindow를 연결하여 드롭 대상 설정
/// 9. FloatingItemBox와 FloatingDescription을 사용하여 아이템 정보를 표시
/// 10. 테스트 버튼을 클릭하면 탈출 성공 여부를 로그로 출력
/// </remarks>

namespace LTH
{
    public class EscapeSocketTestUI : MonoBehaviour
    {
        private VisualElement root;
        private InventoryWindow inventoryWindow;
        private EscapeSocketWindow escapeSocketWindow;
        private ItemBox floatingItemBox;
        private VisualElement floatingDescription;

        public static EscapeSocketWindow Instance { get; private set; }

        public void Awake()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            floatingItemBox = new ItemBox(root);
            floatingItemBox.AddToClassList("item-box-floating");
            floatingItemBox.Hide();
            root.Add(floatingItemBox);

            floatingDescription = CreateFloatingDescription();
            root.Add(floatingDescription);

            // 인벤토리 생성 (전체 아이템 허용)
            inventoryWindow = new InventoryWindow(
                filterItem: item => true,
                floatingItemBox: floatingItemBox,
                floatingDescriptionContainer: floatingDescription
            );
            inventoryWindow.Show();
            root.Add(inventoryWindow);

            // 탈출 소켓 UI 생성
            escapeSocketWindow = new EscapeSocketWindow(floatingItemBox, floatingDescription);
            escapeSocketWindow.Show();
            root.Add(escapeSocketWindow);

            Instance = escapeSocketWindow;

            // 드롭 대상 연결
            inventoryWindow.AddDropTargets(new[] { escapeSocketWindow });

            // 테스트 버튼 추가
            var testButton = new Button(() =>
            {
                bool result = EscapeSocketManager.Instance.CheckEscapeSuccess();
                Debug.Log("탈출 조건 만족 여부: " + result);
            });
            testButton.text = "탈출 성공 여부 확인";
            testButton.style.top = 10;
            testButton.style.left = 10;
            root.Add(testButton);
        }

        private VisualElement CreateFloatingDescription()
        {
            var container = new VisualElement();
            container.AddToClassList("item-storage-item-description-container");

            var title = new Label();
            title.AddToClassList("window-label");
            title.AddToClassList("item-storage-item-description-title");

            var content = new Label();
            content.AddToClassList("item-storage-item-description-content");

            container.Add(title);
            container.Add(content);

            Utils.HideVisualElement(container);
            return container;
        }
    }
}
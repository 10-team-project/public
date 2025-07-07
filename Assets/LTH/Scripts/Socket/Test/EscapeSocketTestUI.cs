using System.Collections;
using System.Collections.Generic;
using SHG;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Ż�� ���� �׽�Ʈ UI Ŭ����
/// </summary>
/// <remarks>
/// �� Ŭ������ Ż�� ���� �׽�Ʈ�� ���� UI�� �����մϴ�.
/// 1. EscapeSocketWindow�� UI �� ����
/// 2. InventoryWindow�� �����Ͽ� �������� �巡�� �� ����� �� �ֵ��� ����
/// 3. FloatingItemBox�� FloatingDescription�� ����Ͽ� ������ ������ ǥ��
/// 4. �׽�Ʈ ��ư�� �߰��Ͽ� Ż�� ���� ���θ� Ȯ���� �� �ֵ��� ��
/// 5. EscapeSocketManager�� ����Ͽ� Ż�� ������ Ȯ��
/// 6. �κ��丮���� �������� �巡�� �� ����Ͽ� Ż�� ���Ͽ� ������ �� �ֵ��� ����
/// 7. EscapeSocketManager�� CheckEscapeSuccess �޼��带 ȣ���Ͽ� Ż�� ������ �����Ǿ����� Ȯ��
/// 8. EscapeSocketWindow�� InventoryWindow�� �����Ͽ� ��� ��� ����
/// 9. FloatingItemBox�� FloatingDescription�� ����Ͽ� ������ ������ ǥ��
/// 10. �׽�Ʈ ��ư�� Ŭ���ϸ� Ż�� ���� ���θ� �α׷� ���
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

            // �κ��丮 ���� (��ü ������ ���)
            inventoryWindow = new InventoryWindow(
                filterItem: item => true,
                floatingItemBox: floatingItemBox,
                floatingDescriptionContainer: floatingDescription
            );
            inventoryWindow.Show();
            root.Add(inventoryWindow);

            // Ż�� ���� UI ����
            escapeSocketWindow = new EscapeSocketWindow(floatingItemBox, floatingDescription);
            escapeSocketWindow.Show();
            root.Add(escapeSocketWindow);

            Instance = escapeSocketWindow;

            // ��� ��� ����
            inventoryWindow.AddDropTargets(new[] { escapeSocketWindow });

            // �׽�Ʈ ��ư �߰�
            var testButton = new Button(() =>
            {
                bool result = EscapeSocketManager.Instance.CheckEscapeSuccess();
                Debug.Log("Ż�� ���� ���� ����: " + result);
            });
            testButton.text = "Ż�� ���� ���� Ȯ��";
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
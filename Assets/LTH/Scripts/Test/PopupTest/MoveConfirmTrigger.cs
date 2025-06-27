using UnityEngine;

namespace LTH
{
    public class MoveConfirmTrigger : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            var popup = PopupManager.Instance.ShowPopup<ConfirmPopup>();

            if (popup == null) return;

            popup.Show("�̵��Ͻðڽ��ϱ�?", "��", () => Debug.Log("Ȯ�� Ŭ��!"), "�ƴϿ�", () => Debug.Log("��� Ŭ��!"));
        }
    }
}

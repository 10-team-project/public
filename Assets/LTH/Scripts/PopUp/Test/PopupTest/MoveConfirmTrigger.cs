using UnityEngine;

namespace LTH
{
    public class MoveConfirmTrigger : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            var popup = PopupManager.Instance.ShowPopup<ConfirmPopup>();

            if (popup == null) return;

            popup.Show("이동하시겠습니까?", "예", () => Debug.Log("확인 클릭!"), "아니요", () => Debug.Log("취소 클릭!"));
        }
    }
}

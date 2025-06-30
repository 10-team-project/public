using UnityEngine;

namespace NTJ
{
    public class SleepInteractTrigger : MonoBehaviour
    {
        public GameTimeManager timeManager;
        public GameObject interactUI; // ��ȣ �ۿ�Ű�� ���ڱ� �ȳ� UI
        private bool isPlayerInRange = false;

        void Update()
        {
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
            {
                timeManager.RequestManualSleep(); // ���� ��û
                interactUI.SetActive(false);     // UI ��Ȱ��ȭ
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;
                interactUI.SetActive(true);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
                interactUI.SetActive(false);
            }
        }
    }
}
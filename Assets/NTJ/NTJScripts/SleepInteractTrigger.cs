using UnityEngine;
using SHG;

namespace NTJ
{
    public class SleepInteractTrigger : MonoBehaviour
    {
        public GameTimeManager timeManager;
        public GameObject interactUI; // ��ȣ �ۿ�Ű�� ���ڱ� �ȳ� UI
        private bool isPlayerInRange = false;

        void Awake()
        {
          timeManager = App.Instance.GameTimeManager;
        }

        void OnEnable()
        {
          App.Instance.GameTimeManager.bedSpawnPoint = this.transform;
        }

        void OnDisable()
        {
          App.Instance.GameTimeManager.bedSpawnPoint = null;
        }

        void Update()
        {
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && interactUI.activeSelf)
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

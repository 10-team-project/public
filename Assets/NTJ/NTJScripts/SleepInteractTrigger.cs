using UnityEngine;
using SHG;

namespace NTJ
{
    public class SleepInteractTrigger : MonoBehaviour
    {
        public GameTimeManager timeManager;
        public GameObject interactUI; // 상호 작용키로 잠자기 안내 UI
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
                timeManager.RequestManualSleep(); // 수면 요청
                interactUI.SetActive(false);     // UI 비활성화
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

using KSH;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace NTJ
{
    public class GameTimeManager : MonoBehaviour
    {
        [Header("UI")]
        public TextMeshProUGUI timeText;           // ��� ���� �ð�
        public TextMeshProUGUI dayText;            // ��� ���� Day-X
        public Image fadePanel;                    // ȭ�� ������ Panel
        public GameObject dayTextPanel;            // �߾ӿ� ��� "Day - X" �г�
        public TextMeshProUGUI dayTextDisplay;     // �߾� �ؽ�Ʈ ("Day - X")
        public CanvasGroup topUITextGroup;
        public Slider timeScaleSlider;
        public TextMeshProUGUI timeScaleText;
        public Animator playerAnimator;            // Project â���� ��Ŭ�� �� Create �� Animator Controller
                                                   // Player�� Animator ������Ʈ�� ���� ������ Add Component �� Animator
        public Transform bedSpawnPoint; // ħ�� ���� ��ġ ����
        public Transform player; // �÷��̾� Transform ����

        [Header("Time Settings")]
        public int timeScale = 30; // ���� 1�� = ���� 1��
        private float gameTime;    // ������ ���� �ð�
        private int currentDay = 1;
        private float fadeDuration = 2f;
        private bool isSleeping = false;
        private bool sleepRequested = false;


        void Start()
        {
#if UNITY_EDITOR
            SaveManager.ClearSave(); // �����Ϳ��� ������ �� ���� ����
#endif
            if (SaveManager.HasSavedData())
            {
                GameData data = SaveManager.LoadData();
                if (data != null)
                {
                    currentDay = data.day; 
                    LoadFromData(data);
                    player.position = bedSpawnPoint.position; // ħ�� ��ġ���� ����
                }
            }
            else
            {
                player.position = bedSpawnPoint.position;
            }

            gameTime = 9 * 3600f;
            UpdateDayText();

            fadePanel.gameObject.SetActive(false);
            fadePanel.color = new Color(0, 0, 0, 0);
            dayTextPanel.SetActive(false);
            timeScaleSlider.onValueChanged.AddListener(OnTimeScaleChanged);
            timeScaleSlider.value = timeScale;
            timeScaleText.text = $"{timeScaleSlider.value}���";
        }

        void Update()
        {
            if (isSleeping) return;

            gameTime += Time.deltaTime * timeScale;

            int hours = (int)(gameTime / 3600) % 24;
            int minutes = (int)(gameTime / 60) % 60;

            timeText.text = string.Format("{0:D2}:{1:D2}", hours, minutes);

            if (hours >= 24 || hours < 9)
            {
                if (!sleepRequested)
                    StartCoroutine(SleepWithAnimation(false)); // ���� ����
            }
        }

        public void RequestManualSleep()
        {
            if (isSleeping || sleepRequested) return;
            StartCoroutine(SleepWithAnimation(true));
        }

        private IEnumerator SleepWithAnimation(bool isManual)
        {
            sleepRequested = true;

            if (playerAnimator != null)
            {
                if (isManual)
                    playerAnimator.SetTrigger("SleepStart");  // ���� ���� �ִϸ��̼� Ʈ����
                else
                    playerAnimator.SetTrigger("FallAsleep");  // ���� ����(������) �ִϸ��̼� Ʈ����

                // �ִϸ��̼� ���̿� �°� ��� 
                yield return new WaitForSeconds(3f);
            }

            // ���� ���� ó�� (Day ����, ü�� ȸ��, ���� ��)
            yield return StartCoroutine(SleepAndStartNextDay(isManual));

            sleepRequested = false;
        }


        void UpdateDayText()
        {
            dayText.text = $"Day - {currentDay}";
        }

        IEnumerator SleepAndStartNextDay(bool isManual)
        {
            isSleeping = true;

            // Fade In
            fadePanel.gameObject.SetActive(true);
            fadePanel.color = new Color(0, 0, 0, 0);

            float t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
                fadePanel.color = new Color(0, 0, 0, alpha);
                topUITextGroup.alpha = 1 - alpha;
                yield return null;
            }

            currentDay++;
            UpdateDayText();

            // �߾� Day-X �ؽ�Ʈ ǥ��
            dayTextPanel.SetActive(true);
            dayTextDisplay.text = $"Day - {currentDay}";
            yield return new WaitForSeconds(1.5f); // �ؽ�Ʈ ���� �ð�

            // ���� �� �ð� ����
            gameTime = 9 * 3600f;

            // ü�� ȸ��
            var stat = PlayerStatManager.Instance;
            var maxHP = stat.HP.resource.Max;
            stat.HP.resource.Cur = isManual ? maxHP * 0.7f : maxHP * 0.3f;

            // �ڵ� ����
            var saveData = CreateSaveData(currentDay);
            SaveManager.SaveData(saveData);

            // Fade Out
            t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(1, 0, t / fadeDuration);
                fadePanel.color = new Color(0, 0, 0, alpha);
                topUITextGroup.alpha = 1 - alpha;
                yield return null;
            }

            fadePanel.gameObject.SetActive(false);
            dayTextPanel.SetActive(false);
            isSleeping = false;

            player.transform.position = bedSpawnPoint.position; // �÷��̾ ħ�뿡�� ������
        }
        public void OnTimeScaleChanged(float value)
        {
            timeScale = Mathf.RoundToInt(value);
            timeScaleText.text = $"{timeScale}���";
        }

        private GameData CreateSaveData(int currentDay)
        {
            var data = new GameData();
            data.day = currentDay;

            var stat = PlayerStatManager.Instance;
            data.hp = stat.HP.resource.Cur;
            data.hunger = stat.Hunger.HungerCur;
            data.thirst = stat.Thirsty.ThirstyCur;
            data.fatigue = stat.Fatigue.FatigueCur;

            data.inventoryItemIDs = Inventory.Instance.GetItemIDs();
            data.quickSlotItemIDs = Inventory.Instance.GetQuickSlotItemIDs();

            return data;
        }
        private void LoadFromData(GameData data)
        {
            var stat = PlayerStatManager.Instance;

            stat.HP.CurrentHP = data.hp;
            stat.Hunger.SetHunger(data.hunger);
            stat.Thirsty.SetThirst(data.thirst);
            stat.Fatigue.SetFatigue(data.fatigue);

            Inventory.Instance.LoadFromItemIDs(data.inventoryItemIDs);
            Inventory.Instance.LoadQuickSlotItems(data.quickSlotItemIDs);
        }
    }

}
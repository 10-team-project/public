using KSH;
using SHG;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EditorAttributes;

namespace NTJ
{
    public class GameTimeManager : MonoBehaviour
    {

        public static GameTimeManager Instance { get; private set; }

        [Header("UI")]
        public TextMeshProUGUI timeText;           // 상단 현재 시간
        public TextMeshProUGUI dayText;            // 상단 현재 Day-X
        public Image fadePanel;                    // 화면 암전용 Panel
        public GameObject dayTextPanel;            // 중앙에 띄울 "Day - X" 패널
        public TextMeshProUGUI dayTextDisplay;     // 중앙 텍스트 ("Day - X")
        public CanvasGroup topUITextGroup;
        public Slider timeScaleSlider;
        public TextMeshProUGUI timeScaleText;
        public Animator playerAnimator;            // Project 창에서 우클릭 → Create → Animator Controller
                                                   // Player에 Animator 컴포넌트를 연결 없으면 Add Component → Animator
        public Transform bedSpawnPoint; // 침대 스폰 위치 지정
        public Transform player; // 플레이어 Transform 참조

        [Header("Time Settings")]
        public int timeScale = 30; // 현실 1초 = 게임 1분
        [SerializeField, ReadOnly]
        private float gameTime;    // 누적된 게임 시간
        private int currentDay = 0;
        public int CurrentDay => currentDay;
        private float fadeDuration = 2f;
        private bool isSleeping = false;
        private bool sleepRequested = false;

        public event Action<int> OnDayChanged;
        public event Action OnSleep;

        void Start()
        {
            if (SaveManager.HasSavedData())
            {
                GameData data = SaveManager.LoadData();
                if (data != null)
                {
                    SetDay(data.day);
                    LoadFromData(data);
                    //player.position = bedSpawnPoint.position + bedSpawnPoint.forward * 1.5f; // 침대 위치에서 시작
                }
            }
            else
            {
                SetDay(1);
                //player.position = bedSpawnPoint.position + bedSpawnPoint.forward * 1.5f;
            }

            gameTime = 9 * 3600f;
            UpdateDayText();

            fadePanel.gameObject.SetActive(false);
            fadePanel.color = new Color(0, 0, 0, 0);
            dayTextPanel.SetActive(false);
            timeScaleSlider.onValueChanged.AddListener(OnTimeScaleChanged);
            timeScaleSlider.value = timeScale;
            timeScaleText.text = $"{timeScaleSlider.value}배속";
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
                    StartCoroutine(SleepWithAnimation(false)); // 강제 수면
            }
        }

        public void RequestManualSleep()
        {
            if (isSleeping || sleepRequested) return;
            StartCoroutine(SleepWithAnimation(true));
        }

        public void SetDay(int newDay)
        {
            if (currentDay != newDay)
            {
                currentDay = newDay;
                UpdateDayText();
                OnDayChanged?.Invoke(currentDay);
            }
        }

        void UpdateDayText()
        {
            dayText.text = $"합숙 <color=red>{currentDay}</color>일차";
        }

        private IEnumerator SleepWithAnimation(bool isManual)
        {
            sleepRequested = true;

            if (playerAnimator != null)
            {
                if (isManual)
                    playerAnimator.SetTrigger("SleepStart");  // 수동 수면 애니메이션 트리거
                else
                    playerAnimator.SetTrigger("FallAsleep");  // 강제 수면(쓰러짐) 애니메이션 트리거

                // 애니메이션 길이에 맞게 대기 
                yield return new WaitForSeconds(3f);
            }

            // 실제 수면 처리 (Day 증가, 체력 회복, 저장 등)
            yield return StartCoroutine(SleepAndStartNextDay(isManual));

            sleepRequested = false;
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

            SetDay(currentDay + 1);

            // 중앙 Day-X 텍스트 표시
            dayTextPanel.SetActive(true);
            dayTextDisplay.text = $"합숙 <color=red>{currentDay}</color>일차";
            yield return new WaitForSeconds(1.5f); // 텍스트 유지 시간

            // 다음 날 시간 리셋
            gameTime = 9 * 3600f;

            // 체력 회복
            var stat = PlayerStatManager.Instance;
            float fatigueMax = stat.Fatigue.FatigueMax;
            float fatigueRecover = isManual ? fatigueMax * 0.7f : fatigueMax * 0.3f;
            stat.Fatigue.Sleep(fatigueRecover);


            // 자동 저장
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
            //player.transform.position = bedSpawnPoint.position; // 플레이어가 침대에서 리스폰
        }
        public void OnTimeScaleChanged(float value)
        {
            timeScale = Mathf.RoundToInt(value);
            timeScaleText.text = $"{timeScale}배속";
        }

        private GameData CreateSaveData(int currentDay)
        {
            var data = new GameData();
            data.day = currentDay;

            var stat = PlayerStatManager.Instance;
            data.hp = stat.HP.CurrentHP;
            data.hunger = stat.Hunger.HungerCur;
            data.thirst = stat.Thirsty.ThirstyCur;
            data.fatigue = stat.Fatigue.FatigueCur;
            data.inventoryItems = App.Instance.Inventory.GetItemSaveDataList();
            data.quickSlotItemIDs = App.Instance.Inventory.GetQuickSlotItemIDs();
            // data.characterID = CharacterManager.Instance.CurrentCharacterID;
            // data.currentTalkID = StoryManager.Instance.GetCurrentTalkID();
            data.lastScene = SceneManager.GetActiveScene().name;
            return data;
        }
        private void LoadFromData(GameData data)
        {
            var stat = PlayerStatManager.Instance;

            App.Instance.Inventory.LoadFromItemSaveDataList(data.inventoryItems);
            App.Instance.Inventory.LoadQuickSlotItems(data.quickSlotItemIDs);

            stat.HP.CurrentHP = data.hp;
            stat.Hunger.SetHunger(data.hunger);
            stat.Thirsty.SetThirst(data.thirst);
            stat.Fatigue.SetFatigue(data.fatigue);

            // StoryManager.Instance?.SetCurrentTalk(data.currentTalkID); // 스토리 진행 복원
            // CharacterStats stats = CharacterDatabase.GetCharacterStatsByID(data.characterID); // 캐릭터 ID기반 능력치 적용
            // if (stats != null)
            // {
            //     stat.HP.SetMaxHP(stats.hp);
            //     stat.Hunger.SetMaxHunger(stats.hunger);
            //     stat.Thirsty.SetMaxThirst(stats.thirsty);
            //     stat.Fatigue.SetMaxFatigue(stats.working);
            // }
        }
    }
}

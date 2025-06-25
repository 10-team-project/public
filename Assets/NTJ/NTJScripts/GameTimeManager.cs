using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace NTJ
{
    public class GameTimeManager : MonoBehaviour
    {
        [Header("UI")]
        public TextMeshProUGUI timeText;           // 상단 현재 시간
        public TextMeshProUGUI dayText;            // 상단 현재 Day-X
        public Image fadePanel;                    // 화면 암전용 Panel
        public GameObject dayTextPanel;            // 중앙에 띄울 "Day - X" 패널
        public TextMeshProUGUI dayTextDisplay;     // 중앙 텍스트 ("Day - X")
        public CanvasGroup topUITextGroup;

        [Header("Time Settings")]
        public int timeScale = 30; // 현실 1초 = 게임 1분
        private float gameTime;    // 누적된 게임 시간
        private int currentDay = 1;

        private bool isSleeping = false;
        private float fadeDuration = 2f;
        public static int InitialDay = 1;

        void Start()
        {
            if (SaveManager.HasSavedData())
            {
                SaveManager.LoadGame();
                currentDay = InitialDay;
            }
            else
            {
                currentDay = 1;
            }

            gameTime = 9 * 3600f;
            UpdateDayText();

            fadePanel.gameObject.SetActive(false);
            fadePanel.color = new Color(0, 0, 0, 0);
            dayTextPanel.SetActive(false);
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
                StartCoroutine(SleepAndStartNextDay(false)); // 강제 수면
            }
        }

        public void OnSleepButtonPressed()
        {
            if (isSleeping)
                StartCoroutine(SleepAndStartNextDay(true)); // 수동 수면
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

            // 중앙 Day-X 텍스트 표시
            dayTextPanel.SetActive(true);
            dayTextDisplay.text = $"Day - {currentDay}";
            yield return new WaitForSeconds(1.5f); // 텍스트 유지 시간

            // 다음 날 시간 리셋
            gameTime = 9 * 3600f;

            // 체력 회복
            float maxHP = GameStateManager.Instance.maxHP;
            GameStateManager.Instance.playerHP = isManual ?
                maxHP * 0.7f : maxHP * 0.3f;

            // 자동 저장
            SaveManager.SaveGame(currentDay);

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
        }
    }
}
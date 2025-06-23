using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


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

        [Header("Time Settings")]
        public int timeScale = 60; // ���� 1�� = ���� 1��
        private float gameTime;    // ������ ���� �ð�
        private int currentDay = 1;

        private bool isSleeping = false;
        private float fadeDuration = 2f;

        void Start()
        {
            gameTime = 9 * 3600f; // ���� 9�ú��� ����
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
                StartCoroutine(SleepAndStartNextDay());
            }
        }

        void UpdateDayText()
        {
            dayText.text = $"Day - {currentDay}";
        }

        IEnumerator SleepAndStartNextDay()
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
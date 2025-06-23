using UnityEngine;

namespace NTJ
{
    public class GameTimeManager : MonoBehaviour
    {
        public int timeMultiplier = 60; // 이 수치가 60이면 1초 = 1분 (수치 변경 가능)
                                        // public Text timeText; // UI 표시용
                                        // public Text dateText; // UI 표시용

        private float gameMinutes; // 누적된 게임 내 분(minute)
        private int day = 1;
        private int month = 1;
        private int year = 1;

        private void Update()
        {
            // 게임 시간 경과 계산
            gameMinutes += Time.deltaTime * timeMultiplier;

            // 하루 24시간 넘었는지
            if (gameMinutes >= 1440)
            {
                gameMinutes -= 1440; // 24시간이 지나면 다음날로
                day++;

                if (day > 30)
                {
                    day = 1;
                    month++;

                    if (month > 12)
                    {
                        month = 1;
                        year++;
                    }
                }
            }
            UpdateTimeUI();
        }

        private void UpdateTimeUI()
        {
            int hours = (int)(gameMinutes / 60) % 24;
            int minutes = (int)(gameMinutes % 60);
            // timeText.text = $"시간: {hours:D2}:{minutes:D2}";
            // dateText.text = $"날짜: {year}년 {month}월 {day}일";
        }
    }
}
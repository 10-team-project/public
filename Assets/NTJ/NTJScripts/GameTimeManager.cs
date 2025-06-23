using UnityEngine;

namespace NTJ
{
    public class GameTimeManager : MonoBehaviour
    {
        public int timeMultiplier = 60; // �� ��ġ�� 60�̸� 1�� = 1�� (��ġ ���� ����)
                                        // public Text timeText; // UI ǥ�ÿ�
                                        // public Text dateText; // UI ǥ�ÿ�

        private float gameMinutes; // ������ ���� �� ��(minute)
        private int day = 1;
        private int month = 1;
        private int year = 1;

        private void Update()
        {
            // ���� �ð� ��� ���
            gameMinutes += Time.deltaTime * timeMultiplier;

            // �Ϸ� 24�ð� �Ѿ�����
            if (gameMinutes >= 1440)
            {
                gameMinutes -= 1440; // 24�ð��� ������ ��������
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
            // timeText.text = $"�ð�: {hours:D2}:{minutes:D2}";
            // dateText.text = $"��¥: {year}�� {month}�� {day}��";
        }
    }
}
using NTJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioEventManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameTimeManager.Instance.OnDayChanged += OnDayChanged;
    }

    private void OnDisable()
    {
        GameTimeManager.Instance.OnDayChanged -= OnDayChanged;
    }

    private void OnDayChanged(int day)
    {
        CheckDailyEvents(day);
    }

    private void CheckDailyEvents(int day)
    {
        // 예: 날짜별 이벤트 분기
        switch (day)
        {
            case 2:
                // 2일차 이벤트 실행
                RunEventForDay2();
                break;
            case 3:
                // 3일차 이벤트 실행
                RunEventForDay3();
                break;
            // 추가 날짜 이벤트
            default:
                break;
        }
    }

    private void RunEventForDay2()
    {
        Debug.Log("2일차 이벤트");
    }

    private void RunEventForDay3()
    {
        Debug.Log("3일차 이벤트");
    }
}

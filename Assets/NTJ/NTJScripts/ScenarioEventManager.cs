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
        // ��: ��¥�� �̺�Ʈ �б�
        switch (day)
        {
            case 2:
                // 2���� �̺�Ʈ ����
                RunEventForDay2();
                break;
            case 3:
                // 3���� �̺�Ʈ ����
                RunEventForDay3();
                break;
            // �߰� ��¥ �̺�Ʈ
            default:
                break;
        }
    }

    private void RunEventForDay2()
    {
        Debug.Log("2���� �̺�Ʈ");
    }

    private void RunEventForDay3()
    {
        Debug.Log("3���� �̺�Ʈ");
    }
}

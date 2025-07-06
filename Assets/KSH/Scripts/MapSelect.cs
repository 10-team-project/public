using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTH;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{
    [Header("Place Panel")]
    [SerializeField] private GameObject Where1;
    [SerializeField] private GameObject Where2;
    [SerializeField] private GameObject Where3;
    [Header("Button")]
    [SerializeField] private Button FirstFloor;
    [SerializeField] private Button ThirdFloor;
    [SerializeField] private Button Cafeteria;
    [SerializeField] private Button Snackbar;
    [SerializeField] private Button Rooftop;
    [Header("Arrow Button")]
    [SerializeField] private Button Where1Next;
    [SerializeField] private Button Where2Next;
    [SerializeField] private Button Where2Back;
    [SerializeField] private Button Where3Back;
    [Header("Exit Button")]
    [SerializeField] private Button[] Exit;

    private bool isPause = false;

    private void Start()
    {
        HideAllPanels();
        Interact();
        MoveEvent();
    }

    public void MoveEvent() //이벤트
    {
        FirstFloor.onClick.AddListener(() => ExitPanel(Where1));
        ThirdFloor.onClick.AddListener(() => ExitPanel(Where1));
        Cafeteria.onClick.AddListener(() => ExitPanel(Where2));
        Snackbar.onClick.AddListener(() => ExitPanel(Where2));
        Rooftop.onClick.AddListener(() => ExitPanel(Where3));

        Where1Next.onClick.AddListener(() => SwitchPanel(Where1, Where2));
        Where2Next.onClick.AddListener(() => SwitchPanel(Where2, Where3));
        Where2Back.onClick.AddListener(() => SwitchPanel(Where2, Where1));
        Where3Back.onClick.AddListener(() => SwitchPanel(Where3, Where2));
        
        for (int i = 0; i < Exit.Length; i++)
        {
            int index = i;
            Exit[i].onClick.AddListener(() =>
            {
                HideAllPanels();
                Time.timeScale = 1f;
                isPause = false;
            });
        }
    }

    public void Interact() //나가시겠습니까? 팝업창
    {
        var popup = PopupManager.Instance.ShowPopup<ConfirmPopup>();
        if (popup == null) return;

        popup.Show("", "",
            () =>
            {
                Where1.SetActive(true);
                Time.timeScale = 0f;
                isPause = true;
            },

            "",
            () => { }
        );
    }
    
    private void ExitPanel(GameObject currentPanel) //맵 버튼 누르면 창 사라지는 기능
    {
        currentPanel.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
    }

    private void SwitchPanel(GameObject CurPanel, GameObject BeforePanel) //화살표 표시 누르면 창 바뀌는 기능
    {
        CurPanel.SetActive(false);
        BeforePanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        Where1.SetActive(false);
        Where2.SetActive(false);
        Where3.SetActive(false);
    }
}

using NTJ;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private const string DEFAULT_GAME_SCENE = "NTJTestScene";
    private const string CHARACTER_CHOICE_SCENE = "CharacterChoiceScene";

    [Header("세이브 정보")]
    public GameObject loadInfoPanel;
    public Image portraitImage;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI hpText, hungerText, thirstText, fatigueText;

    private GameData cachedData;

    public void OnStartGame()
    {
        SceneManager.LoadScene(CHARACTER_CHOICE_SCENE);
    }

    public void OnContinueGame()
    {
        if (SaveManager.HasSavedData())
        {
            cachedData = SaveManager.LoadData();
            ShowLoadInfo(cachedData);
        }
        else
        {
            Debug.Log("세이브 데이터 없음");
        }
    }

    private void ShowLoadInfo(GameData data)
    {
        dayText.text = $"합숙 <color=red>{data.day}</color>일차";
        hpText.text = $"체력: {data.hp:F0}";
        hungerText.text = $"허기: {data.hunger:F0}";
        thirstText.text = $"갈증: {data.thirst:F0}";
        fatigueText.text = $"피로: {data.fatigue:F0}";

        // 캐릭터 초상화 ID 추가
        //  portraitImage.sprite = CharacterManager.Instance.GetPortraitData(data.characterID);

        loadInfoPanel.SetActive(true);
    }

    public void OnConfirmLoad()
    {
        string sceneToLoad = string.IsNullOrEmpty(cachedData.lastScene) ? DEFAULT_GAME_SCENE : cachedData.lastScene;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnCancelLoadInfo()
    {
        loadInfoPanel.SetActive(false);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}

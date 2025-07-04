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

    [Header("���̺� ����")]
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
            Debug.Log("���̺� ������ ����");
        }
    }

    private void ShowLoadInfo(GameData data)
    {
        dayText.text = $"�ռ� <color=red>{data.day}</color>����";
        hpText.text = $"ü��: {data.hp:F0}";
        hungerText.text = $"���: {data.hunger:F0}";
        thirstText.text = $"����: {data.thirst:F0}";
        fatigueText.text = $"�Ƿ�: {data.fatigue:F0}";

        // ĳ���� �ʻ�ȭ ID �߰�
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

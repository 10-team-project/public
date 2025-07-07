using KSH;
using NTJ;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private const string DEFAULT_GAME_SCENE = "NTJTestScene";
    private const string CHARACTER_CHOICE_SCENE = "CharacterChoiceScene";

    [SerializeField] private GameObject optionPanel;

    [Header("���̺� ����")]
    public GameObject loadInfoPanel;
    public TextMeshProUGUI dayText;

    private GameData cachedData;

    private void Update()
    {
        if (OptionUIManager.isPause) return; // �ɼ� ���������� ����
    }

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

    public void OnOpenOption()
    {
        optionPanel.SetActive(true);
    }

    public void OnCloseOption()
    {
        optionPanel.SetActive(false);
    }
}

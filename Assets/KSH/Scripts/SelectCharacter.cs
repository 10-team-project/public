using System;
using System.Collections;
using System.Collections.Generic;
using KSH;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KSH
{
    enum CharacterType
    {
        Character1, Character2
    }
    public class SelectCharacter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image CenterImage;
        [SerializeField] private TextMeshProUGUI TitleText;
        [SerializeField] private TextMeshProUGUI DescriptionText;
        [SerializeField] private CharacterType CharacterType;
        private Image image;
        private SceneManager sceneManager;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        private void UIAlpha(float amount)
        {
            Alpha(image, amount);
            Alpha(CenterImage, amount);
            Alpha(TitleText, amount);
            Alpha(DescriptionText, amount);
        }

        private void Alpha(Graphic graphic, float amount)
        {
            Color color = graphic.color;
            color.a = amount;
            graphic.color = color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UIAlpha(1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIAlpha(0.5f);
        }

        public void OnClickButton()
        {
            PlayerPrefs.SetInt("PlayerSelect", (int)CharacterType); // 클릭하면 해당 UI의 캐릭터 타입을 저장
            KSH.TestSceneManager.Instance.MainGameScene("Test");
        }
    }
}

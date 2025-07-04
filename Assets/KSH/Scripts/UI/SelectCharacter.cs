using System;
using System.Collections;
using System.Collections.Generic;
using KSH;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SHG;

namespace KSH
{
    enum CharacterType
    {
        GangHana, Seonahyun
    }
    public class SelectCharacter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler //이 기능은 마우스가 ui위에 있는지 없는지 판단
    {
        [Header("Text BackGround Image")]
        [SerializeField] private Image CenterImage; //텍스트 배경 이미지
        [Header("Character Type")]
        [SerializeField] private CharacterType CharacterType; //캐릭터 타입
        
        private Image image; //캐릭터 이미지
        private SceneManager sceneManager; //씬매니저

        private void Start()
        {
            image = GetComponent<Image>(); //이미지 컴포넌트 받아오기
        }

        private void UIAlpha(float amount)
        {
            Alpha(image, amount);
            Alpha(CenterImage, amount);
        }

        private void Alpha(Graphic graphic, float amount) // 알파 값 가져오는 기능
        {
            Color color = graphic.color;
            color.a = amount;
            graphic.color = color;
        }
        // 투명화는 0~1 , 1로 갈수록 선명
        public void OnPointerEnter(PointerEventData eventData)
        {
            UIAlpha(1f); //투명화 최소
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIAlpha(0.5f); //투명화 중간
        }

        public void OnClickButton() //버튼 눌렀을 때
        {
          var mode = App.Instance.CurrentMode as CharacterSelectMode;
          PlayerPrefs.SetInt("PlayerSelect", (int)CharacterType); // 클릭하면 해당 UI의 캐릭터 타입을 저장
          mode.OnSelectCharacter(null);
          return ;
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Test");

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;
using UnityEngine.UI;

namespace KSH
{
    public class OptionUIManager : SingletonBehaviour<OptionUIManager>
    {
        [SerializeField] private GameObject ParseUI; //일시정지 창
        [SerializeField] private Button OptionIconButton; //옵션 아이콘 버튼
        [SerializeField] private Button ContinueButton; //계속하기 버튼
        [SerializeField] private Button OptionButton; //옵션 버튼
        [SerializeField] private Button ExitButton; //나가기 버튼
        [SerializeField] private Button OptionExitButton; //옵션 창 나가기 버튼

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            ClickOption();
        }
        
        private void ClickOption()
        {
            OptionIconButton.onClick.AddListener(() =>
            {
                ParseUI.SetActive(true);
                //게임 멈추기
            });
            
            OptionExitButton.onClick.AddListener(() =>
            {
                ParseUI.SetActive(false);
            });
            
            ContinueButton.onClick.AddListener(() =>
            {
                ParseUI.SetActive(false);
            });
        }
    }    
}

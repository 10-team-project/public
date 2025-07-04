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
        [SerializeField] private GameObject KeyUI; //조작키 창
        [SerializeField] private GameObject SoundUI; //사운드 창
        [SerializeField] private Button OptionIconButton; //옵션 아이콘 버튼
        [SerializeField] private Button ContinueButton; //계속하기 버튼
        [SerializeField] private Button OptionButton; //옵션 버튼
        [SerializeField] private Button ExitButton; //나가기 버튼
        [SerializeField] private Button[] OptionExitButton; //옵션 창 나가기 버튼
        [SerializeField] private Button KeyButton; //조작키 버튼
        [SerializeField] private Button SoundButton; //사운드 버튼
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            ClickOption();
        }
        
        public void ClickOption()
        {
            OptionIconButton.onClick.AddListener(() =>
            {
                ParseUI.SetActive(true);
                //게임 멈추기
            });

            for (int i = 0; i < OptionExitButton.Length; i++)
            {
                int index = i;
                
                OptionExitButton[i].onClick.AddListener(() =>
                {
                    switch (index)
                    {
                        case 0:
                            ParseUI.SetActive(false);
                            break;
                        case 1:
                            KeyUI.SetActive(false);
                            break;
                        case 2:
                            SoundUI.SetActive(false);
                            break;
                    }
                });
            }
            
            ContinueButton.onClick.AddListener(() =>
            {
                ParseUI.SetActive(false);
            });

            OptionButton.onClick.AddListener(() =>
            {
                ParseUI.SetActive(false);
                KeyUI.SetActive(true);
            });
            
            KeyButton.onClick.AddListener(() =>
            {
                KeyUI.SetActive(true);
                SoundUI.SetActive(false);
            });
            
            SoundButton.onClick.AddListener(() =>
            {
                KeyUI.SetActive(false);
                SoundUI.SetActive(true);
            });
        }
    }    
}

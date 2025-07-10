using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using KSH;

namespace KSH
{
    public class OptionPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text optionText; //버튼 안 선택지 대사

        private OptionNode.OptionData curOptionData; //현재 선택지의 데이터
        private Action<OptionNode.OptionData> endCallback; //콜백 이벤트

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClickOption);
        }

        public void SetOptionData(OptionNode.OptionData oData, Action<OptionNode.OptionData> endCb)
        {
            curOptionData = oData;

            optionText.text = curOptionData.text;
            endCallback = endCb;
        }

        public void OnClickOption()
        {
            endCallback?.Invoke(curOptionData);
        }
    }
}

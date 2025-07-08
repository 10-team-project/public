using System.Collections;
using System.Collections.Generic;
using LTH;
using UnityEngine;

namespace LTH
{
    public class EscapeExit : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            if (!EscapeManager.Instance.IsEscapeReady)
            {
                Debug.Log("아직 탈출 조건이 충족되지 않았습니다.");
                TriggerEscapeFailure();
                return;
            }
            Debug.Log("탈출 상호작용 시작됨");
            StartEscapeSequence();
        }

        // 비상문 상호작용 후 연출시작
        private void StartEscapeSequence()
        {
            Debug.Log("탈출 연출 시작됨");
            // TODO: 게임 클리어 이벤트(대사), 해피엔딩 UI 출력 등 탈출 후 처리 로직 작성
        }

        private void TriggerEscapeFailure()
        {
            Debug.Log("탈출 실패 연출 시작");
            // TODO: 게임 오버 이벤트(대사), 배드엔딩 UI 등 게임 오버 후 처리 로직 작성
        }
    }
}
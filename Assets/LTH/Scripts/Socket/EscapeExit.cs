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
                Debug.Log("���� Ż�� ������ �������� �ʾҽ��ϴ�.");
                TriggerEscapeFailure();
                return;
            }
            Debug.Log("Ż�� ��ȣ�ۿ� ���۵�");
            StartEscapeSequence();
        }

        // ��� ��ȣ�ۿ� �� �������
        private void StartEscapeSequence()
        {
            Debug.Log("Ż�� ���� ���۵�");
            // TODO: ���� Ŭ���� �̺�Ʈ(���), ���ǿ��� UI ��� �� Ż�� �� ó�� ���� �ۼ�
        }

        private void TriggerEscapeFailure()
        {
            Debug.Log("Ż�� ���� ���� ����");
            // TODO: ���� ���� �̺�Ʈ(���), ��忣�� UI �� ���� ���� �� ó�� ���� �ۼ�
        }
    }
}
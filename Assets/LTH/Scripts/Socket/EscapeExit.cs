using System.Collections;
using System.Collections.Generic;
using KSH;
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
                TriggerEscapeFailure();
                return;
            }
            StartEscapeSequence();
        }

        // ��� ��ȣ�ۿ� �� �������
        private void StartEscapeSequence()
        {
            Debug.Log("Ż�� ���� ���۵�");

            // ���ǿ��� ��� ����
            ScriptManager.Instance.StartScript(10601);

            // TODO: ���� Ŭ���� �̺�Ʈ(���), ���ǿ��� UI ��� �� Ż�� �� ó�� ���� �ۼ�
        }

        private void TriggerEscapeFailure()
        {
            Debug.Log("Ż�� ���� ���� ����");

            // ��忣�� ��� ����
            ScriptManager.Instance.StartScript(10501);

            // TODO: ���� ���� �̺�Ʈ(���), ��忣�� UI �� ���� ���� �� ó�� ���� �ۼ�
        }
    }
}
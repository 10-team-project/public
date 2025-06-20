using System.Collections;
using System.Collections.Generic;
using LTH;
using UnityEngine;
using static UnityEditor.Progress;

namespace LTH
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] float interactRange; // ��ȣ�ۿ� ����

        [SerializeField] string[] interactLayerNames; // ��ȣ�ۿ��� �� �� �ִ� ���̾� ���� (�� ������ �ƴϱ� ������ �迭�� ��ġ)
        private LayerMask interactLayer; // 

        [SerializeField] Camera cam; // �÷��̾��� ���� ī�޶� ���̸� ��� ���� �ʿ���

        // [SerializeField] TMP_Text interactionText; // �ӽ� UI

        [SerializeField] float interactionCooldown = 0.5f; // �������� ������ ȹ�� ���� ��Ÿ��
        private float lastInteractionTime = float.MinValue;


        private void Update()
        {
            LookItem();
        }


        private void LookItem()
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            if (Physics.SphereCast(ray, 0.3f, out RaycastHit hit, interactRange, interactLayer))
            {
                Item item = hit.collider.GetComponent<Item>();
                if (item != null)
                {
                    // UI ���� ������ ��ȣ�ۿ븸 ó��
                    if (Input.GetKeyDown(KeyCode.E) && Time.time - lastInteractionTime > interactionCooldown)
                    {
                        lastInteractionTime = Time.time;
                        item.Interact();
                    }
                }
            }
        }
    }
}




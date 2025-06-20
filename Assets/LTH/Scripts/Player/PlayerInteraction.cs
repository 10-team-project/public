using System.Collections;
using System.Collections.Generic;
using LTH;
using UnityEngine;
using static UnityEditor.Progress;

namespace LTH
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] float interactRange; // 상호작용 범위

        [SerializeField] string[] interactLayerNames; // 상호작용을 볼 수 있는 레이어 설정 (한 가지가 아니기 때문에 배열로 배치)
        private LayerMask interactLayer; // 

        [SerializeField] Camera cam; // 플레이어의 시점 카메라 레이를 쏘기 위해 필요함

        // [SerializeField] TMP_Text interactionText; // 임시 UI

        [SerializeField] float interactionCooldown = 0.5f; // 연속으로 아이템 획득 방지 쿨타임
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
                    // UI 없이 아이템 상호작용만 처리
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




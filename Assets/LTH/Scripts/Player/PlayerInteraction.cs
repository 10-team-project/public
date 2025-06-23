using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactRange; // 상호작용 범위

    [SerializeField] string[] interactLayerNames; // 상호작용을 볼 수 있는 레이어 설정 (한 가지가 아니기 때문에 배열로 배치)
    private LayerMask interactLayer; // 

    // [SerializeField] private Transform eyeTransform; // 눈 위치 (헤드, 머리 위치 등)

    [SerializeField] float interactionCooldown = 0.5f; // 연속으로 아이템 획득 방지 쿨타임
    private float lastInteractionTime = float.MinValue;


    private void Start()
    {
        interactLayer = LayerMask.GetMask(interactLayerNames);
    }

    private void Update()
    {
        LookItem();
    }


    private void LookItem()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactLayer);

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                Debug.Log($"아이템 {hit.gameObject.name} E키를 눌러 줍기");

                if (Input.GetKeyDown(KeyCode.E) && Time.time - lastInteractionTime > interactionCooldown)
                {
                    lastInteractionTime = Time.time;
                    interactable.Interact();
                    break;
                }
            }
        }

        //Ray ray = new Ray(eyeTransform.position, eyeTransform.forward);

        //if (Physics.SphereCast(ray, 0.3f, out RaycastHit hit, interactRange, interactLayer))
        //{
        //    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
        //    if (interactable != null)
        //    {
        //        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastInteractionTime > interactionCooldown)
        //        {
        //            lastInteractionTime = Time.time;
        //            interactable.Interact();
        //        }
        //    }
        //}
    }

    private void OnDrawGizmos()
    {
       // if (eyeTransform == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}




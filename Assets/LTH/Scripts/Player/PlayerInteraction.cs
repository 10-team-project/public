using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using LTH;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactRange; // 상호작용 범위

    [SerializeField] string[] interactLayerNames; // 상호작용을 볼 수 있는 레이어 설정 (한 가지가 아니기 때문에 배열로 배치)
    private LayerMask interactLayer;

    [SerializeField] float interactionCooldown = 0.5f; // 연속으로 아이템 획득 방지 쿨타임
    private float lastInteractionTime = float.MinValue;


    private void Start()
    {
        interactLayer = LayerMask.GetMask(interactLayerNames);
    }

    private void Update()
    {
        AllInteraction();
    }


    private void AllInteraction()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactLayer);

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                Debug.Log($"{hit.gameObject.name} F키를 눌러 상호작용");

                if (Input.GetKeyDown(KeyCode.F) && Time.time - lastInteractionTime > interactionCooldown)
                {
                    lastInteractionTime = Time.time;
                    interactable.Interact();
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}




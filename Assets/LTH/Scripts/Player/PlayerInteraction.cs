using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactRange; // ��ȣ�ۿ� ����

    [SerializeField] string[] interactLayerNames; // ��ȣ�ۿ��� �� �� �ִ� ���̾� ���� (�� ������ �ƴϱ� ������ �迭�� ��ġ)
    private LayerMask interactLayer; // 

    // [SerializeField] private Transform eyeTransform; // �� ��ġ (���, �Ӹ� ��ġ ��)

    [SerializeField] float interactionCooldown = 0.5f; // �������� ������ ȹ�� ���� ��Ÿ��
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
                Debug.Log($"������ {hit.gameObject.name} EŰ�� ���� �ݱ�");

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




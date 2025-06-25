using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;
using LTH;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactRange;

    [SerializeField] string[] interactLayerNames;
    private LayerMask interactLayer;

    [SerializeField] float interactionCooldown = 0.5f;
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
                //Debug.Log($"{hit.gameObject.name} F키를 눌러 상호작용");

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




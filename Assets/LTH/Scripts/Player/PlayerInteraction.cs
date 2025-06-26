using UnityEngine;
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
        if (InputManager.Instance.IsBlocked(InputType.Interaction)) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactLayer);

        IInteractable closest = null;
        float closestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = interactable;
                }
            }
        }

        if (closest != null && Input.GetKeyDown(KeyCode.F) && Time.time - lastInteractionTime > interactionCooldown)
        {
            lastInteractionTime = Time.time;
            closest.Interact();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}




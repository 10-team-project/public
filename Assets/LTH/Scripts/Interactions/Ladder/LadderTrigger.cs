using UnityEngine;

public enum LadderTriggerType
{
    EnterBottom,
    EnterTop,
    ExitBottom,
    ExitTop
}

public class LadderTrigger : MonoBehaviour, IInteractable
{
    [HideInInspector] public LadderTriggerType triggerType;
    [SerializeField] public float enterDistance = 1.2f;
    [SerializeField] private bool isLeftLadder = false;

    private PlayerMovement player;

    private void Awake()
    {
        switch (gameObject.tag)
        {
            case "LadderUpStart":
                triggerType = LadderTriggerType.EnterBottom;
                break;
            case "LadderUpEnd":
                triggerType = LadderTriggerType.ExitTop;
                break;
            case "LadderDownStart":
                triggerType = LadderTriggerType.EnterTop;
                break;
            case "LadderDownEnd":
                triggerType = LadderTriggerType.ExitBottom;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerMovement>();

            if (triggerType == LadderTriggerType.ExitTop)
            {
                player.SetAtLadderTop(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerType == LadderTriggerType.ExitTop)
            {
                player?.SetAtLadderTop(false);
            }

            player = null;
        }
    }

    public void Interact()
    {
        if (player == null) return;

        Vector3 climbDirection = isLeftLadder ? Vector3.left : Vector3.right;

        switch (triggerType)
        {
            case LadderTriggerType.EnterBottom:
            case LadderTriggerType.EnterTop:
                player.StartClimbing(transform.position, climbDirection, triggerType);
                break;

            case LadderTriggerType.ExitBottom:
            case LadderTriggerType.ExitTop:
                if (player.IsOnLadder)
                    player.EndClimb();
                break;
        }
    }
}
